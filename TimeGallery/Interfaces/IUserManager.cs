﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeGallery.Models;

namespace TimeGallery.Interfaces
{

    public interface IUserManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

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
    }
}
