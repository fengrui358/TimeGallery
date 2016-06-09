using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using TimeGallery.Interfaces;

namespace TimeGallery.Managers
{
    public class LoadBalanceManager : ILoadBalanceManager
    {
        public IPAddress GetDbHost()
        {
            //todo：现阶段只需要一台主机
            return IPAddress.Parse("127.0.0.1");
        }

        public string GetDbConnection(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            //todo：现阶段只需要一台主机
            return
                $"Server ={ipAddress}; Database =timegallerycontent; Uid =root; Pwd =P@$$w0rd;Pooling=true; Max Pool Size=20;Min Pool Size=10;Allow Batch=true;";
        }
    }
}