namespace TimeGallery.Enums
{
    public enum ContentTypeDefine
    {
        None,

        Image,

        Video
    }

    public static class ContentTypeDefineExtension
    {
        public static string GetString(this ContentTypeDefine contentType)
        {
            switch (contentType)
            {
                case ContentTypeDefine.Image:
                    return "image/jpeg";
                case ContentTypeDefine.Video:
                    return "video/quicktime";
            }

            return string.Empty;
        }

        public static ContentTypeDefine ContentTypeConvert(string type)
        {
            if (!string.IsNullOrEmpty(type))
            {
                if (type.StartsWith("image"))
                {
                    return ContentTypeDefine.Image;
                }
                else if(type.StartsWith("video"))
                {
                    return ContentTypeDefine.Video;
                }
            }

            return ContentTypeDefine.None;
        }
    }
}
