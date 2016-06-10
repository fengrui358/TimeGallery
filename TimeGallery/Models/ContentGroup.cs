using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeGallery.DataBase.Entity;

namespace TimeGallery.Models
{
    public class ContentGroup : ContentGroupDbEntity
    {
        public IEnumerable<ContentModel> ContentModels { get; set; }
    }
}