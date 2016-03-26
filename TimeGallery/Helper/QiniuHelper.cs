using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Qiniu.RS;

namespace TimeGallery.Helper
{
    public class QiniuHelper
    {
        public static string GetToken()
        {
            if (ConfigHelper.ConfigModel != null && ConfigHelper.ConfigModel.QiniuAccountModels.Any())
            {
                //todo:负载均衡的选择账户
                var account = ConfigHelper.ConfigModel.QiniuAccountModels.First();

                return GetToken(account.AccessKey, account.SecretKey, account.Bucket);
            }
            else
            {
                LogManager.GetCurrentClassLogger().Error("获取七牛Token的账户不存在");
                return null;
            }
        }

        private static string GetToken(string accessKey, string secretKey, string bucket)
        {
            try
            {
                Qiniu.Conf.Config.ACCESS_KEY = accessKey;
                Qiniu.Conf.Config.SECRET_KEY = secretKey;

                var put = new PutPolicy(bucket, 3600);

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
