using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeGallery.Interfaces;

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
                    "Server =localhost; Database =timegallery; Uid =root; Pwd =P@$$w0rd;Pooling=true; Max Pool Size=20;Min Pool Size=10;Allow Batch=true;",
                    "MySql.Data.MySqlClient");
            }

            return null;
        }
    }
}
