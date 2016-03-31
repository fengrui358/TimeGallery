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
                var qiniuAccount = ConfigHelper.ConfigModel.QiniuAccountModels.First();

                return qiniuAccount.GetToken();
            }
            else
            {
                LogManager.GetCurrentClassLogger().Error("获取七牛Token的账户不存在");
                return null;
            }
        }
    }
}
