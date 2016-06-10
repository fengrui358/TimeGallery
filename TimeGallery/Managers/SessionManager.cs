using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI;
using Newtonsoft.Json;
using NLog;
using TimeGallery.Consts;
using TimeGallery.DataBase.Entity;
using TimeGallery.Interfaces;
using TimeGallery.Models;

namespace TimeGallery.Managers
{
    /// <summary>
    /// todo:没用原生的Session管理器，先自己实现个简单的
    /// todo:将Session管理内置到Controller的基类去，用特性来标记不需要Session的方法
    /// </summary>
    public class SessionManager : ISessionManager
    {
        private readonly ConcurrentDictionary<Guid, SessionModel> _sessionDictionary = new ConcurrentDictionary<Guid, SessionModel>();

        /// <summary>
        /// 在线用户，此处在线只是网页在线，并不是指在公众号的会话当中
        /// </summary>
        private readonly ConcurrentDictionary<string, Guid> _onLineUserDictionary = new ConcurrentDictionary<string, Guid>(); 

        private readonly IUserManager _userManager;

        public static Guid TempUserSession = Guid.NewGuid();

        public SessionManager(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 校验Session是否有效
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public bool VerifySession(HttpContextBase httpContext)
        {
            var sessionGuid = GetSessionFromCookie(httpContext);
            if (_sessionDictionary.ContainsKey(sessionGuid))
            {
                _sessionDictionary[sessionGuid].Refresh();
                return true;
            }

            return false;
        }

        public Guid AddSession(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                throw new ArgumentNullException(nameof(openId));
            }

            if (_onLineUserDictionary.ContainsKey(openId))
            {
                _sessionDictionary[_onLineUserDictionary[openId]].Refresh();

                return _onLineUserDictionary[openId];
            }
            else
            {
                var user = _userManager.GetUser(openId);
                if (user != null)
                {
                    var newSession = new SessionModel(user);
                    newSession.ExpiresEvent += SessionOnExpiresEventHandler;

                    //判断该用户是否有服务端还没过期的Session记录，有则清除
                    if (_onLineUserDictionary.ContainsKey(user.OpenId))
                    {                        
                        Guid existSessionId;
                        if (_onLineUserDictionary.TryRemove(user.OpenId, out existSessionId))
                        {
                            SessionModel outSession;
                            _sessionDictionary.TryRemove(existSessionId, out outSession);
                        }
                    }

                    if (!_sessionDictionary.TryAdd(newSession.Id, newSession) ||
                        !_onLineUserDictionary.TryAdd(user.OpenId, newSession.Id))
                    {
                        LogManager.GetCurrentClassLogger()
                        .Error($"用户Session添加失败，用户OpenId：{openId}");
                    }

                    return newSession.Id;
                }
            }

            return TempUserSession;
        }

        public UserModel GetOnlineUser(Guid sessionId)
        {
            //if (sessionId == Guid.Empty)
            //{
            //    throw new ArgumentNullException(nameof(sessionId));
            //}

            if (_sessionDictionary.ContainsKey(sessionId))
            {
                return _sessionDictionary[sessionId].UserModel;
            }

            return null;
        }

        /// <summary>
        /// 从Cookie当中获取Session值
        /// </summary>
        /// <param name="httpContextBase"></param>
        /// <returns>如果不存在则返回Guid.Empty</returns>
        public Guid GetSessionFromCookie(HttpContextBase httpContextBase)
        {
            if (httpContextBase.Session[SystemString.SessionKey] != null)
            {
                Guid outGuid;
                if (Guid.TryParse(httpContextBase.Session[SystemString.SessionKey].ToString(), out outGuid))
                {
                    return outGuid;
                }
            }

            return Guid.Empty;
        }

        private void SessionOnExpiresEventHandler(object sender, EventArgs eventArgs)
        {
            var session = (SessionModel) sender;
            if (_sessionDictionary.ContainsKey(session.Id))
            {
                SessionModel outSession;
                if (!_sessionDictionary.TryRemove(session.Id, out outSession))
                {
                    LogManager.GetCurrentClassLogger()
                        .Error($"用户Session移除失败，相关信息：{JsonConvert.SerializeObject(session)}");
                }
            }

            if (_onLineUserDictionary.ContainsKey(session.UserModel.OpenId))
            {
                Guid outGuid;
                if (!_onLineUserDictionary.TryRemove(session.UserModel.OpenId, out outGuid))
                {
                    LogManager.GetCurrentClassLogger()
                        .Error($"用户Session移除失败，相关信息：{JsonConvert.SerializeObject(session)}");
                }
            }
        }
    }
}