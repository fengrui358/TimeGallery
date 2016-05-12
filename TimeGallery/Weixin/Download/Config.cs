using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeGallery.Weixin.Download
{
    public class Config
    {
        public int QrCodeId { get; set; }
        public List<string> Versions { get; set; }
        public int DownloadCount { get; set; }
    }
}