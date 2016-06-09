using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Dapper.FastCrud;
using NLog;
using Senparc.Weixin.Helpers;
using TimeGallery.Enums;

namespace TimeGallery.DataBase.Entity
{
    [Table("user")]
    public class UserDbEntity
    {
        /// <summary>
        /// OpenID（微信账号针对每个公众号的唯一ID）
        /// </summary>
        [Key]
        public virtual string OpenId { get; set; }

        /// <summary>
        /// 主键
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        /// </summary>
        public virtual string Uuid { get; set; }

        /// <summary>
        /// 用户的加入平台的时间序号
        /// </summary>
        [Key]
        [DatabaseGeneratedDefaultValue]
        public virtual int OrderNumber { get; set; }

        /// <summary>
        /// 微信用户自身的昵称
        /// </summary>
        public virtual string NickName { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public virtual string HeadImgUrl { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public virtual UserSexTypeDefine Sex { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public virtual string Province { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public virtual string Country { get; set; }

        /// <summary>
        /// 用户的语言，zh-CN 简体，zh-TW 繁体，en 英语，默认为zh-CN
        /// </summary>
        public virtual string Language { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息
        /// </summary>
        public virtual bool Subscribe { get; set; }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public virtual long SubscribeTime { get; set; }

        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        public virtual int GroupId { get; set; }

        /// <summary>
        /// 是否曾经注册过相册
        /// </summary>
        public virtual bool RegisteredGallery { get; set; }

        /// <summary>
        /// 获取指定大小的用户头像网址
        /// </summary>
        /// <param name="size">代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像）</param>
        /// <returns></returns>
        public string GetHeadImageUrl(int size = 0)
        {
            var url = HeadImgUrl;
            if (url == null)
                return null;

            if (size != 0 && size != 46 && size != 64 && size != 96 && size != 132)
            {
                LogManager.GetCurrentClassLogger().Error($"头像参数：{size}不在指定范围内");
                size = 0;
            }

            var tail = $"/{size.ToString("d")}";
            if (url.EndsWith(tail))
                return url;

            var slashIndex = url.LastIndexOf('/');
            if (slashIndex < 0)
                return url;

            return $"{url.Substring(0, slashIndex)}{tail}";
        }

        /// <summary>
        /// 订阅时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetSubscribeTime()
        {
            return DateTimeHelper.GetDateTimeFromXml(SubscribeTime);
        }
    }
}