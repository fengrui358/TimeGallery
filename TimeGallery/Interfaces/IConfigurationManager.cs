using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeGallery.Interfaces
{
    public interface IConfigurationManager : IDependency
    {
        string GetAppSetting(string key);        

        ConnectionStringSettings GetConnectionString(string key = "");

        /// <summary>
        /// 主机名
        /// </summary>
        string HostName { get; }

        /// <summary>
        /// 网站名
        /// </summary>
        string WebName { get; }

        /// <summary>
        /// 网站名（友好标题）
        /// </summary>
        string WebTitle { get; }

        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        ConnectionStringSettings DefaultConnectionString { get; }
    }
}
