using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeGallery.Enums;

namespace TimeGallery.DataBase.Entity
{
    /// <summary>
    /// 用户及相册关系
    /// </summary>
    [Table("user_gallery_rel")]
    public class UserGalleryRelDbEntity
    {
        /// <summary>
        /// 用户openId
        /// </summary>
        [Key]
        public virtual string OpenId { get; set; }

        /// <summary>
        /// 相册Id
        /// </summary>
        [Key]
        public virtual long GalleryId { get; set; }

        /// <summary>
        /// 用户及相册关系
        /// </summary>
        public virtual UserGalleryRelTypeDefine UserGalleryRelType { get; set; }
    }
}