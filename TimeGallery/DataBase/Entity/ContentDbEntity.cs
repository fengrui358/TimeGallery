using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Senparc.Weixin.Annotations;

namespace TimeGallery.DataBase.Entity
{
    [Table("content")]
    public class ContentDbEntity
    {
        [Key]        
        public virtual Guid Id { get; set; }

        public virtual Guid ContentGroupId { get; set; }

        public virtual long GalleryId { get; set; }

        public virtual string Type { get; set; }

        public virtual string Url { get; set; }

        public virtual long Size { get; set; }

        [DatabaseGeneratedDefaultValue]
        public virtual DateTime CreateTime { get; set; }
    }
}
