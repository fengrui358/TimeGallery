using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using TimeGallery.Interfaces;

namespace TimeGallery.Managers
{
    public class WebConfigConfigurationManager : IConfigurationManager
    {
        public string GetAppSetting(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

        public ConnectionStringSettings GetConnectionString(string key = "")
        {
            return string.IsNullOrEmpty(key)
                ? WebConfigurationManager.ConnectionStrings["MySqlConnString"]
                : WebConfigurationManager.ConnectionStrings[key];
        }

#if DEBUG
        public string HostName => "http://fengrui358.vicp.cc";
#else
        public string HostName => GetAppSetting("HostName");
#endif

#if DEBUG
        public string WebName => "http://fengrui358.vicp.cc/TimeGallery";
#else
        public string WebName => GetAppSetting("WebName");
#endif

        public string WebTitle => GetAppSetting("WebTitle");

        public ConnectionStringSettings DefaultConnectionString => GetConnectionString("MySqlConnString");
    }
}