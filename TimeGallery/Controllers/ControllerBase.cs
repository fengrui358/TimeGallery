using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeGallery.Consts;
using TimeGallery.Interfaces;
using TimeGallery.Models;

namespace TimeGallery.Controllers
{
    public class ControllerBase : Controller
    {
        protected IConfigurationManager ConfigurationManager;
        protected IUserManager UserManager;
        protected ISessionManager SessionManager;
        protected IGalleryManager GalleryManager;

        /// <summary>
        /// 获取发起当前请求的用户信息
        /// </summary>
        protected UserModel CurrentUserModel
            => SessionManager.GetOnlineUser(SessionManager.GetSessionFromCookie(HttpContext));

        public ControllerBase(IConfigurationManager configurationManager, IUserManager userManager, ISessionManager sessionManager, IGalleryManager galleryManager)
        {
            ConfigurationManager = configurationManager;
            UserManager = userManager;
            SessionManager = sessionManager;
            GalleryManager = galleryManager;
        }        
    }
}