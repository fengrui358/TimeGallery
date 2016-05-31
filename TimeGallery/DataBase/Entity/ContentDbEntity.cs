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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }

        public virtual string Type { get; set;}

        public virtual string Url { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DatabaseGeneratedDefaultValue]
        public virtual DateTime CreateTime { get; set; }

        public virtual string Size { get; set; }

        public virtual string Description { get; set; }
    }
}
