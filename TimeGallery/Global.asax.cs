using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using NLog;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Managers;
using TimeGallery.Weixin;

namespace TimeGallery
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LogManager.GetCurrentClassLogger().Info("服务启动");
            Error += OnError;            

            IocHelper.Init();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitManagers();
        }

        private void InitManagers()
        {
            IocHelper.Container.Resolve<IUserManager>().Init();
            IocHelper.Container.Resolve<IGalleryManager>().Init();
            IocHelper.Container.Resolve<IWeixinManager>().Init();
        }

        /// <summary>
        /// 未处理的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnError(object sender, EventArgs eventArgs)
        {
            //todo:改成更通用的module捕获异常的方法 http://www.cnblogs.com/youring2/archive/2012/04/25/2469974.html
            //获取到HttpUnhandledException异常，这个异常包含一个实际出现的异常
            Exception ex = Server.GetLastError();
            LogManager.GetCurrentClassLogger().Error(ex);
        }
    }
}
