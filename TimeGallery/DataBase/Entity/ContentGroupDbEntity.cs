using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeGallery.DataBase.Entity
{
    [Table("contentgroup")]
    public class ContentGroupDbEntity
    {
        /// <summary>
        /// Guid主键
        /// </summary>
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 所属相册外键
        /// </summary>
        public virtual long GalleryId { get; set; }

        /// <summary>
        /// 该分组的图片数量
        /// </summary>
        public virtual int ImageCount { get; set; }

        /// <summary>
        /// 该分组的视频数量
        /// </summary>
        public virtual int VideoCount { get; set; }

        /// <summary>
        /// 该分组是哪一天
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
}