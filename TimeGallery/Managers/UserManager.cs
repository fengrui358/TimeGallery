using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Dapper.FastCrud;
using TimeGallery.DataBase;
using TimeGallery.Interfaces;
using TimeGallery.Models;

namespace TimeGallery.Managers
{
    public class UserManager : IUserManager
    {
        private Dictionary<string, UserModel> _weixinFollowerUsers;
        
        public void Init()
        {
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

            _weixinFollowerUsers = users.ToDictionary(s => s.Uuid);
        }

        public async void AddUser(UserModel userModel)
        {
            if (!_weixinFollowerUsers.ContainsKey(userModel.Uuid))
            {
                _weixinFollowerUsers.Add(userModel.Uuid, userModel);

                using (var con = StorageHelper.GetConnection())
                {
                    await con.InsertAsync(userModel);
                }
            }
            else
            {
                //todo:判断用户信息是否需要更新
            }
        }
    }
}