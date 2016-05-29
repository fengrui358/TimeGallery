using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using TimeGallery.Interfaces;

namespace TimeGallery.Helper
{
    public class SimpleUrlHelper
    {
        private const string UrlSeparator = "/";
        private const string ControllerPostfix = "Controller";
        private static readonly int ControllerPostfixLength = ControllerPostfix.Length;

        public static Lazy<IConfigurationManager> ConfigManagerLazy =
            new Lazy<IConfigurationManager>(() => IocHelper.Container.Resolve<IConfigurationManager>());

        /// <summary>
        /// 通过nameof控制器和探测器名字来组装强名称Url
        /// todo: 考虑使用UrlHelper.GenerateUrl替换
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GenerateUrl(string controller, string action)
        {
            if (string.IsNullOrEmpty(controller))
            {
                return ConfigManagerLazy.Value.WebName;
            }

            if (controller.EndsWith(ControllerPostfix, StringComparison.OrdinalIgnoreCase))
            {
                controller = controller.Remove(controller.Length - ControllerPostfixLength, ControllerPostfixLength);
            }

            return string.Join(UrlSeparator, ConfigManagerLazy.Value.WebName, controller, action ?? string.Empty);
        }
    }
}