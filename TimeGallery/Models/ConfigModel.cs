using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Qiniu.RS;

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

        /// <summary>
        /// 根据配置获取上传Token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            try
            {
                Qiniu.Conf.Config.ACCESS_KEY = AccessKey;
                Qiniu.Conf.Config.SECRET_KEY = SecretKey;

                var put = new PutPolicy(Bucket, 3600);

                //调用Token()方法生成上传的Token
                string upToken = put.Token();

                return upToken;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return null;
            }
        }
    }
}
