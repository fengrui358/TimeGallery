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
    }
}
