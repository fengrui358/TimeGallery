using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using NLog;
using Senparc.Weixin.MP.CommonAPIs;
using TimeGallery.DataBase;
using TimeGallery.Interfaces;
using TimeGallery.Models;
using TimeGallery.Weixin;

namespace TimeGallery.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IConfigurationManager _configurationManager;
        private ConcurrentDictionary<string, UserModel> _usersDictionary;

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

            _usersDictionary = new ConcurrentDictionary<string, UserModel>(users.ToDictionary(s => s.OpenId));

            LogManager.GetCurrentClassLogger().Info($"用户管理器初始化完毕，当前用户共{_usersDictionary.Count}位");
        }

        public async void AddUser(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            if (!_usersDictionary.ContainsKey(userModel.OpenId))
            {
                //添加新用户信息到缓存字典
                if (_usersDictionary.TryAdd(userModel.OpenId, userModel))
                {
                    LogManager.GetCurrentClassLogger().Info($"新增用户：{JsonConvert.SerializeObject(userModel)}");

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
                    LogManager.GetCurrentClassLogger().Error($"新增用户失败：{JsonConvert.SerializeObject(userModel)}");
                }
            }
            else
            {
                TryUpdateUserInfo(userModel);
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

            if (!_usersDictionary.ContainsKey(userModel.OpenId))
            {
                AddUser(userModel);
                return;
            }

            await Task.Run(async () =>
            {
                //判断最后一次更新时间大于阈值才更新
                var user = _usersDictionary[userModel.OpenId];
                double tryUpdateUserInfoInterval;

                if (!double.TryParse(_configurationManager.GetAppSetting("TryUpdateUserInfoInterval"),
                    out tryUpdateUserInfoInterval))
                {
                    //默认阈值
                    tryUpdateUserInfoInterval = 18000;
                    LogManager.GetCurrentClassLogger().Error("配置项TryUpdateUserInfoInterval出现异常，无法转换为double型");
                }

                if (DateTime.Now.Subtract(user.LastUpDateTime).TotalSeconds > tryUpdateUserInfoInterval)
                {
                    LogManager.GetCurrentClassLogger()
                        .Info(
                            $"更新用户信息，原数据：{JsonConvert.SerializeObject(user)}，新数据：{JsonConvert.SerializeObject(userModel)}");

                    user.LastUpDateTime = DateTime.Now;
                    TinyMapper.Map(userModel, user);

                    using (var con = StorageHelper.GetConnection())
                    {
                        await con.UpdateAsync(user);
                    }
                }
            });
        }

        /// <summary>
        /// 尝试更新用户信息
        /// </summary>
        /// <param name="openId"></param>
        public async void TryUpdateUserInfo(string openId)
        {
            if (string.IsNullOrEmpty(openId))
            {
                throw new ArgumentNullException(nameof(openId));
            }

            if (_usersDictionary.ContainsKey(openId))
            {
                //判断最后一次更新时间大于阈值才更新
                var user = _usersDictionary[openId];
                double tryUpdateUserInfoInterval;

                if (!double.TryParse(_configurationManager.GetAppSetting("TryUpdateUserInfoInterval"),
                    out tryUpdateUserInfoInterval))
                {
                    //默认阈值
                    tryUpdateUserInfoInterval = 1800;
                    LogManager.GetCurrentClassLogger().Error("配置项TryUpdateUserInfoInterval出现异常，无法转换为double型");
                }

                if (DateTime.Now.Subtract(user.LastUpDateTime).TotalSeconds > tryUpdateUserInfoInterval)
                {
                    user.LastUpDateTime = DateTime.Now;

                    //向微信服务器获取用户信息
                    //获取用户信息            
                    var weixinUserInfo = CommonApi.GetUserInfo(WeixinManager.AppId, openId);

                    LogManager.GetCurrentClassLogger()
                        .Info(
                            $"更新用户信息，原数据：{JsonConvert.SerializeObject(user)}，新数据：{JsonConvert.SerializeObject((UserModel) weixinUserInfo)}");

                    if (weixinUserInfo != null)
                    {
                        TinyMapper.Map((UserModel)weixinUserInfo, user);

                        using (var con = StorageHelper.GetConnection())
                        {
                            await con.UpdateAsync(user);
                        }
                    }
                }
            }
        }
    }
}