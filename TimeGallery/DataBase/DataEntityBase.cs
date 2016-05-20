using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Autofac;
using Dapper;
using MySql.Data.MySqlClient;
using NLog;
using TimeGallery.Helper;
using TimeGallery.Interfaces;

namespace TimeGallery.DataBase
{
    public abstract class DataEntityBase<T> where T : class
    {
        private Lazy<PropertyInfo[]> _properties = new Lazy<PropertyInfo[]>(() => typeof(T).GetProperties(BindingFlags.Public));

        protected virtual string TableName
        {
            get
            {
                var className = typeof(T).Name;
                if (className.EndsWith("DbEntity", StringComparison.CurrentCultureIgnoreCase))
                {
                    return className.Remove(className.Length - "DbEntity".Length);
                }
                else
                {
                    return className;
                }
            }
        }

        protected virtual string DataBaseName => GetConnection().Database;

        protected virtual IDbConnection GetConnection()
        {
            try
            {
                //从数据库提取数据到内存做缓存
                var connectStr = IocHelper.Container.Resolve<IConfigurationManager>().GetConnectionString("MySqlConnString");
                return new MySqlConnection(connectStr.ConnectionString);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Fatal("数据库建立连接失败");
                LogManager.GetCurrentClassLogger().Fatal(ex);
                throw;
            }
        }

        public void Save()
        {
            try
            {

            }
            catch (Exception exception)
            {
                LogManager.GetCurrentClassLogger().Error(exception);
            }
        }

        public void Update()
        {
            try
            {

            }
            catch (Exception exception)
            {
                LogManager.GetCurrentClassLogger().Error(exception);
            }
        }

        public void SaveOrUpdate()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append($"INSERT INTO `{DataBaseName}`.`{TableName}`");

                using (var con = GetConnection())
                {
                    var c =
                        "INSERT INTO `timegallery`.`content` (`Type`, `Url`, `CreateTime`, `Size`, `Description`) VALUES (@Type, @Url, @CreateTime, @Size, @Description);";
                    //con.Execute()
                }
            }
            catch (Exception exception)
            {
                LogManager.GetCurrentClassLogger().Error(exception);
            }
        }
    }
}