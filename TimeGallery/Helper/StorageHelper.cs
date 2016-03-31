﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Dapper;
using MySql.Data.MySqlClient;
using NLog;
using TimeGallery.DataBase.Entity;

namespace TimeGallery.Helper
{
    /// <summary>
    /// 存储辅助
    /// </summary>
    public class StorageHelper
    {
        public static bool InsertContent(ContentDbEntity contentDbEntity)
        {
            try
            {
                if (contentDbEntity == null)
                {
                    throw new ArgumentNullException(nameof(contentDbEntity));
                }

                using (var connection = GetConnection())
                {
                    int result = connection.Execute(ContentDbEntity.InsertSql(), contentDbEntity);

                    if (result != 1)
                    {
                        LogManager.GetCurrentClassLogger().Error("数据插入失败");
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            return false;
        }

        public static IEnumerable<ContentDbEntity> Search(DateTime afterDateTime, int pageSize = -1)
        {
            var result = new List<ContentDbEntity>();

            try
            {
                using (var connection = GetConnection())
                {
                    //todo: 还未仔细查询
                    result = connection.Query<ContentDbEntity>(ContentDbEntity.SearchAllSql()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            return result;
        }

        private static IDbConnection GetConnection()
        {
            try
            {
                //从数据库提取数据到内存做缓存
                var connectStr = WebConfigurationManager.ConnectionStrings["MySqlConnString"];

                return new MySqlConnection(connectStr.ConnectionString);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Fatal("数据库建立连接失败");
                LogManager.GetCurrentClassLogger().Fatal(ex);
                throw;
            }
        }
    }
}
