using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeGallery.Interfaces;
using TimeGallery.Tests.Helper;

namespace TimeGallery.Tests.Fakes
{
    public class StubWebConfigConfigurationManager : IConfigurationManager
    {
        public string GetAppSetting(string key)
        {
            throw new NotImplementedException();
        }

        public ConnectionStringSettings GetConnectionString(string key = "")
        {
            if (key == "MySqlConnString")
            {
                return new ConnectionStringSettings("MySqlConnString",
                    $"Server =localhost; Database ={DbTestHelper.FakeDataBaseName}; Uid =root; Pwd =P@$$w0rd;Pooling=true; Max Pool Size=20;Min Pool Size=10;Allow Batch=true;",
                    "MySql.Data.MySqlClient");
            }

            return null;
        }

        public string HostName => "http://fengrui358.vicp.cc";

        public string WebName => "http://fengrui358.vicp.cc/TimeGallery";

        public string WebTitle => GetAppSetting("测试时光相册");

        public ConnectionStringSettings DefaultConnectionString => GetConnectionString("MySqlConnString");
    }
}
