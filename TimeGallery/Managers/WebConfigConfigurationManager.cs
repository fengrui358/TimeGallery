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
    }
}