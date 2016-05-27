using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime CreateTime { get; set; }

        public virtual string Size { get; set; }

        public virtual string Description { get; set; }

        public static string SearchAllSql()
        {
            return "SELECT * FROM `timegallery`.`content` LIMIT 1000;";
        }

        public static string InsertSql()
        {
            return
                "INSERT INTO `timegallery`.`content` (`Type`, `Url`, `CreateTime`, `Size`, `Description`) VALUES (@Type, @Url, @CreateTime, @Size, @Description);";
        }
    }
}
