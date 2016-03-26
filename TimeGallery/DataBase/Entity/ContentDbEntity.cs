using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeGallery.Enum;

namespace TimeGallery.DataBase.Entity
{
    public class ContentDbEntity
    {
        public virtual long Id { get; set; }

        public virtual ContentTypeDefine Type { get; set;}

        public virtual string Url { get; set; }

        public virtual DateTime CreateTime { get; set; }

        public virtual string Description { get; set; }
    }
}
