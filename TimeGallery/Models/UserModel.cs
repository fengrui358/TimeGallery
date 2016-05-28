﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Entities;
using TimeGallery.DataBase.Entity;

namespace TimeGallery.Models
{
    public class UserModel : UserDbEntity
    {
        /// <summary>
        /// 上一次更新用户信息的时间
        /// </summary>
        public DateTime LastUpDateTime { get; set; }

        public string GetGalleryDbConnectingString()
        {
            if (IsManager)
            {
                //todo:返回分布式的内容数据信息
                return string.Empty;
            }

            return string.Empty;
        }

        public static explicit operator UserModel(WeixinUserInfoResult weixinUserInfo)
        {
            if (weixinUserInfo == null)
            {
                throw new ArgumentNullException(nameof(weixinUserInfo));
            }

            var userModel = new UserModel
            {
                OpenId = weixinUserInfo.openid,
                Uuid = weixinUserInfo.unionid,
                IsFollower = weixinUserInfo.subscribe != 0,
                Name = weixinUserInfo.nickname
            };

            return userModel;
        }
    }
}