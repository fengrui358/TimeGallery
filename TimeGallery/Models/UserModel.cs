using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Entities;
using TimeGallery.DataBase.Entity;

namespace TimeGallery.Models
{
    public class UserModel : UserDbEntity
    {
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
                Uuid = string.IsNullOrEmpty(weixinUserInfo.unionid) ? weixinUserInfo.openid : weixinUserInfo.unionid,
                OpenId = weixinUserInfo.openid,
                IsFollower = weixinUserInfo.subscribe != 0,
                Name = weixinUserInfo.nickname
            };

            return userModel;
        }
    }
}