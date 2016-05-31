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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        /// <summary>
        /// 封面Url
        /// </summary>
        public virtual string Cover { get; set; }

        [DatabaseGeneratedDefaultValue]
        public DateTime CreateTime { get; set; }
    }
}