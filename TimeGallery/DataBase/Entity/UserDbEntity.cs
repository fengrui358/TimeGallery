using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeGallery.DataBase.Entity
{
    public class UserDbEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual string Uuid { get; set; }

        /// <summary>
        /// OpenID（微信账号针对每个公众号的唯一ID）
        /// </summary>
        public virtual string OpenId { get; set; }

        /// <summary>
        /// 微信号名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public virtual bool IsManager { get; set; }

        /// <summary>
        /// 是否是关注者
        /// </summary>
        public virtual bool IsFollower { get; set; }
    }
}