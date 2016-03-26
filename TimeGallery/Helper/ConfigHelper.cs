using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Xml.Serialization;
using NLog;
using TimeGallery.Models;

namespace TimeGallery.Helper
{
    public class ConfigHelper
    {
        private static ConfigModel _configModel;

        /// <summary>
        /// 网站标题
        /// </summary>
        public static string WebTitle => WebConfigurationManager.AppSettings["WebTitle"];

        /// <summary>
        /// 自定义的配置实体
        /// </summary>
        public static ConfigModel ConfigModel
        {
            get { return _configModel; }
            private set { _configModel = value; }
        }

        static ConfigHelper()
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");

                ConfigModel = DeserializeConfigModel(configPath);

                var fileSystemWatcher = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");
                fileSystemWatcher.Changed += (sender, args) =>
                {
                    ConfigModel = DeserializeConfigModel(args.FullPath);
                };

                fileSystemWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Fatal(ex);
                LogManager.GetCurrentClassLogger().Fatal("解析核心配置文件Config.xml失败");
                throw;
            }
        }

        private static ConfigModel DeserializeConfigModel(string configPath)
        {
            try
            {
                using (var streamReader = new StreamReader(configPath))
                {
                    var xmlSerializer = new XmlSerializer(typeof(ConfigModel));

                    ConfigModel = (ConfigModel)xmlSerializer.Deserialize(streamReader);
                    return ConfigModel;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Fatal(ex);
                LogManager.GetCurrentClassLogger().Fatal("解析核心配置文件Config.xml失败");
                throw;
            }
        }
    }
}
