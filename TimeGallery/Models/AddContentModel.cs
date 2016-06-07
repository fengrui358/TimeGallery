using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeGallery.DataBase.Entity;

namespace TimeGallery.Models
{
    public class AddContentModel
    {
        public string SourceLink { get; set; }

        public string Type { get; set; }

        public long Size { get; set; }

        public DateTime CreateTime { get; private set; }

        public AddContentModel()
        {
            CreateTime = DateTime.Now;
        }

        public static explicit operator ContentDbEntity(AddContentModel addContentModel)
        {
            return new ContentDbEntity
            {
                Url = addContentModel.SourceLink,
                CreateTime = addContentModel.CreateTime,
                Type = addContentModel.Type,
                Size = addContentModel.Size
            };
        }
    }
}
