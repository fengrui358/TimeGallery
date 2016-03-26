using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Dapper;
using MySql.Data.MySqlClient;
using NLog;
using TimeGallery.DataBase.Entity;

namespace TimeGallery.DataBase
{
    public class DbContext
    {
        public DbContext()
        {
            var connectStr = WebConfigurationManager.ConnectionStrings["MySqlConnString"];
            var connection = new MySqlConnection(connectStr.ConnectionString);

            var content = connection.Query<ContentDbEntity>("select * from content");
            LogManager.GetCurrentClassLogger().Info(content);
        }
    }
}
