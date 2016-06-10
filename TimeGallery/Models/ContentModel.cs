using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeGallery.DataBase.Entity;
using TimeGallery.Enums;

namespace TimeGallery.Models
{
    public class ContentModel : ContentDbEntity
    {
        public ContentTypeDefine GeContentType()
        {
            if (string.IsNullOrEmpty(Type))
            {
                return ContentTypeDefine.None;
            }

            return ContentTypeDefineExtension.ContentTypeConvert(Type);
        }
    }
}