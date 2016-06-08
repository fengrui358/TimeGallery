using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TimeGallery.Interfaces
{
    /// <summary>
    /// 负载数据库的维护，判断当前最空闲的数据库，对没有工作的数据库发出警告
    /// </summary>
    public interface ILoadBalanceManager
    {
        /// <summary>
        /// 获取最合适的数据库主机地址
        /// </summary>
        /// <returns></returns>
        IPAddress GetDbHost();

        /// <summary>
        /// 获取最合适的数据库主机的链接字符串
        /// </summary>
        /// <param name="ipAddress">根据IP地址获取连接字符串</param>
        /// <returns></returns>
        string GetDbConnection(string ipAddress);
    }
}
