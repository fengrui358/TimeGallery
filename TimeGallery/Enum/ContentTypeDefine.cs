using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeGallery.Enum
{
    public enum ContentTypeDefine
    {
        Image
    }

    public static class ContentTypeDefineExtension
    {
        public static string GetString(this ContentTypeDefine contentType)
        {
            switch (contentType)
            {
                case ContentTypeDefine.Image:
                    return "image/jpeg";
            }

            return string.Empty;
        }
    }
}
