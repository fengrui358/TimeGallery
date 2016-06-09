using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Nelibur.ObjectMapper;
using Senparc.Weixin.MP.Entities;
using TimeGallery.DataBase.Entity;
using TimeGallery.Enums;

namespace TimeGallery.Models
{
    public class UserModel : UserDbEntity, IEquatable<UserModel>
    {
        /// <summary>
        /// 上一次更新用户信息的时间
        /// </summary>
        [NotMapped]
        public DateTime LastUpDateTime { get; set; }

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
                NickName = weixinUserInfo.nickname,
                HeadImgUrl = weixinUserInfo.headimgurl,
                Sex = (UserSexTypeDefine)weixinUserInfo.sex,
                City = weixinUserInfo.city,
                Province = weixinUserInfo.province,
                Country = weixinUserInfo.country,
                Language = weixinUserInfo.language,
                Subscribe = weixinUserInfo.subscribe != 0,
                SubscribeTime = weixinUserInfo.subscribe_time,
                Remark = weixinUserInfo.remark,
                GroupId = weixinUserInfo.groupid,
                LastUpDateTime = DateTime.Now
            };

            return userModel;
        }

        public static implicit operator string(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            return userModel.OpenId;
        }

        public bool Equals(UserModel other)
        {
            if (other == null)
            {
                return false;
            }

            bool result =
                !(OpenId != other.OpenId || Uuid != other.Uuid || NickName != other.NickName ||
                  HeadImgUrl != other.HeadImgUrl || Sex != other.Sex || City != other.City
                  || Province != other.Province || Country != other.Country || Language != other.Language ||
                  Subscribe != other.Subscribe || SubscribeTime != other.SubscribeTime
                  || Remark != other.Remark || GroupId != other.GroupId);

            return result;
        }
    }
}