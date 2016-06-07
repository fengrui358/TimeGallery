using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Dapper.FastCrud;
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
        /// 微信用户自身的昵称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 用户的加入平台的时间序号
        /// </summary>
        [DatabaseGeneratedDefaultValue]
        public virtual int OrderNumber { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public virtual UserSexTypeDefine Sex { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public virtual string City { get; set; }
    }    
}