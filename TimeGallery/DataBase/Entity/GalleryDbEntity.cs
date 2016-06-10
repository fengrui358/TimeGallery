using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Dapper.FastCrud;

namespace TimeGallery.DataBase.Entity
{
    [Table("gallery")]
    public class GalleryDbEntity
    {
        /// <summary>
        /// 整型自增主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }

        /// <summary>
        /// 相册名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 相册描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 封面Url
        /// </summary>
        public virtual string CoverUrl { get; set; }

        /// <summary>
        /// 内容表所在数据库的主机地址
        /// </summary>
        public virtual string ContentDbHost { get; set; }

        /// <summary>
        /// 相册中的图片数量
        /// </summary>
        public virtual int TotalImageCount { get; set; }

        /// <summary>
        /// 相册中的视屏数量
        /// </summary>
        public virtual int TotalVideoCount { get; set; }

        /// <summary>
        /// 总大小
        /// </summary>
        public virtual long TotalSize { get; set; }

        /// <summary>
        /// 最后一次上传内容时间，用以判断相册活跃度
        /// </summary>
        public virtual DateTime LastUpdateTime { get; set; }

        [DatabaseGeneratedDefaultValue]
        public DateTime CreateTime { get; set; }
    }
}