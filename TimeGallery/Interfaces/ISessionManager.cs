using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TimeGallery.Models;

namespace TimeGallery.Interfaces
{
    public interface ISessionManager : IDependency
    {
        /// <summary>
        /// 校验Session，如果存在则刷新有效期
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>Session是否存在</returns>
        bool VerifySession(HttpContextBase httpContext);

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="openId">微信用户OpenId</param>
        /// <returns>对应的Session</returns>
        Guid AddSession(string openId);

        /// <summary>
        /// 根据Session值获取当前在线用户
        /// </summary>
        /// <param name="sessionId">SessionId</param>
        /// <returns>不存在则返回null</returns>
        UserModel GetOnlineUser(Guid sessionId);

        /// <summary>
        /// 从Cookie当中获取Session值
        /// </summary>
        /// <param name="httpContextBase"></param>
        /// <returns>如果不存在则返回Guid.Empty</returns>
        Guid GetSessionFromCookie(HttpContextBase httpContextBase);
    }
}
