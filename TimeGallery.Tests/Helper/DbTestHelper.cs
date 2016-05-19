using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using TimeGallery.Tests.Fakes;

namespace TimeGallery.Tests.Helper
{
    public class DbTestHelper
    {
        private const string ConnectStr = "Server =localhost; Uid =root; Pwd =P@$$w0rd;Pooling=true; Max Pool Size=20;Min Pool Size=10;Allow Batch=true;";

        public const string FakeDataBaseName = "faketimegallery";

        /// <summary>
        /// 创建伪数据库
        /// </summary>
        public static void CreateFakeDataBase()
        {
            using (var con = new MySqlConnection(ConnectStr))
            {
                //创建数据库
                con.Execute($"CREATE DATABASE IF NOT EXISTS `{FakeDataBaseName}`;");
                con.Execute($"USE `{FakeDataBaseName}`;");

                //创建表
            }

        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        public static void DropFakeDataBase()
        {
            using (var con = new MySqlConnection(ConnectStr))
            {
                con.Execute($"drop database if exists `{FakeDataBaseName}`;");
            }
        }
    }
}
