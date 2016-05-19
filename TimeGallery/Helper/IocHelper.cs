using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TimeGallery.Interfaces;
using TimeGallery.Managers;

namespace TimeGallery.Helper
{
    public class IocHelper
    {
        public static IContainer Container { get; internal set; }

        /// <summary>
        /// 向Ioc容器注册管理器或服务
        /// </summary>
        public static void Init()
        {
            var builder = new ContainerBuilder();

            //todo：以后改成自动注册
            //var types =
            //    AppDomain.CurrentDomain.GetAssemblies()
            //        .SelectMany(s => s.GetTypes().Where(t => t.IsAssignableFrom(typeof(IDependency)) && !t.IsAbstract));

            builder.RegisterType<WebConfigConfigurationManager>().As<IConfigurationManager>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}