﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Dapper.FastCrud;
using NLog;
using TimeGallery.DataBase;
using TimeGallery.Interfaces;
using TimeGallery.Models;

namespace TimeGallery.Managers
{
    public class UserManager : IUserManager
    {
        private IConfigurationManager _configurationManager;
        private Dictionary<string, UserModel> _weixinFollowerUsers;

        public UserManager(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public void Init()
        {
            LogManager.GetCurrentClassLogger().Info("初始化用户管理器");
            IEnumerable<UserModel> users;

            using (var con = StorageHelper.GetConnection())
            {
                //todo：只筛选关注用户
                //users = con.Find<UserModel>(
                //    statement =>
                //        statement.Where($"{nameof(UserModel.IsWeixinFollower):C}=@IsWeixinFollower")
                //            .WithParameters(new { IsWeixinFollower = true }));

                users = con.Find<UserModel>();
            }

            _weixinFollowerUsers = users.ToDictionary(s => s.OpenId);

            LogManager.GetCurrentClassLogger().Info($"用户管理器初始化完毕，当前用户共{_weixinFollowerUsers.Count}位");
        }

        public async void AddUser(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            if (!_weixinFollowerUsers.ContainsKey(userModel.OpenId))
            {
                //添加新用户信息到缓存字典
                _weixinFollowerUsers.Add(userModel.OpenId, userModel);

                await Task.Run(async () =>
                {
                    using (var con = StorageHelper.GetConnection())
                    {
                        await con.InsertAsync(userModel);
                    }
                });
            }
            else
            {
                //todo:判断用户信息是否需要更新

            }
        }

        /// <summary>
        /// 尝试更新用户信息
        /// </summary>
        /// <param name="userModel"></param>
        public async void TryUpdateUserInfo(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            if (!_weixinFollowerUsers.ContainsKey(userModel.OpenId))
            {
                AddUser(userModel);
                return;
            }

            //判断最后一次更新时间大于阈值才更新
        }
    }
}