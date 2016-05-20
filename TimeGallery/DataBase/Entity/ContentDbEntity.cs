using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.Annotations;
using TimeGallery.DataBase.Attributes;

namespace TimeGallery.DataBase.Entity
{
    public class ContentDbEntity : DataEntityBase<ContentDbEntity>
    {
        [AutoIncrementkey]
        public virtual long Id { get; set; }

        public virtual string Type { get; set;}

        public virtual string Url { get; set; }

        [AutoValue]
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
