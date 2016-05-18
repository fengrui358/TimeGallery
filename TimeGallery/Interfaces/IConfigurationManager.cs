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
    }
}
