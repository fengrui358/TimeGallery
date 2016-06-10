using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Autofac;
using Dapper;
using Dapper.FastCrud;
using MySql.Data.MySqlClient;
using NLog;
using TimeGallery.DataBase.Entity;
using TimeGallery.Helper;
using TimeGallery.Interfaces;

namespace TimeGallery.DataBase
{
    public class StorageHelper
    {
        static StorageHelper()
        {
            OrmConfiguration.DefaultDialect = SqlDialect.MySql;
        }
        
        public static IDbConnection GetConnection()
        {
            try
            {
                //从数据库提取数据到内存做缓存
                var connectStr = IocHelper.Container.Resolve<IConfigurationManager>().DefaultConnectionString;
                return new MySqlConnection(connectStr.ConnectionString);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Fatal("数据库建立连接失败");
                LogManager.GetCurrentClassLogger().Fatal(ex);
                throw;
            }
        }

        public static IDbConnection GetConnection(string dbHostAddress)
        {
            try
            {
#if DEBUG
                return new MySqlConnection(
                    $"Server ={dbHostAddress}; Database =timegallerycontent; Uid =root; Pwd =P@$$w0rd;Pooling=true; Max Pool Size=20;Min Pool Size=10;Allow Batch=true;");
#endif
                return new MySqlConnection(
                                    $"Server ={dbHostAddress}; Database =timegallerycontent; Uid =root; Pwd =P@$$w0rd;Pooling=true; Max Pool Size=20;Min Pool Size=10;Allow Batch=true;");

            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Fatal("数据库建立连接失败");
                LogManager.GetCurrentClassLogger().Fatal(ex);
                throw;
            }
        }

        public static IDbConnection GetConnection(GalleryDbEntity galleryDbEntity)
        {
            return GetConnection(galleryDbEntity.ContentDbHost);
        }
    }
}