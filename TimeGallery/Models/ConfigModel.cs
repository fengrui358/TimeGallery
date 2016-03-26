using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeGallery.Models
{
    public class ConfigModel
    {
        public List<QiniuAccountModel> QiniuAccountModels { get; set; }
    }

    public class QiniuAccountModel
    {
        public string Name { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        /// <summary>
        /// 空间名称
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
    }
}
