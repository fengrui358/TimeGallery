using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI;
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
        private IUserManager _userManager;

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
            if (httpContext.Session[ConstInfos.SessionKey] != null)
            {
                var sessionGuid = (Guid) httpContext.Session[ConstInfos.SessionKey];
                if (_sessionDictionary.ContainsKey(sessionGuid))
                {
                    _sessionDictionary[sessionGuid].LastRefreshTime = DateTime.Now;
                    return true;
                }
            }            

            return false;
        }

    }
}