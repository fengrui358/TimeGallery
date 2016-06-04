using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeGallery.Models;

namespace TimeGallery.Interfaces
{

    public interface IUserManager : IManagerDependency
    {
        /// <summary>
        /// 获取用户信息的拷贝
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        UserModel GetUser(string openId);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userModel"></param>
        void AddUser(UserModel userModel);

        /// <summary>
        /// 尝试更新用户信息
        /// </summary>
        /// <param name="userModel"></param>
        void TryUpdateUserInfo(UserModel userModel);

        /// <summary>
        /// 尝试更新用户信息
        /// </summary>
        /// <param name="openId"></param>
        void TryUpdateUserInfo(string openId);        
    }
}
