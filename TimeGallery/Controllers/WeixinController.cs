using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using NLog;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using TimeGallery.Interfaces;
using TimeGallery.Models;
using TimeGallery.Weixin;

namespace TimeGallery.Controllers
{
    public class WeixinController : Controller
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IUserManager _userManager;
        readonly Func<string> _getRandomFileName = () => DateTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);

        public WeixinController(IConfigurationManager configurationManager, IUserManager userManager)
        {
            _configurationManager = configurationManager;
            _userManager = userManager;
        }

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WeixinManager.Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return
                    Content(
                        $"failed: {postModel.Signature}, {CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, WeixinManager.Token)} 如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WeixinManager.Token))
            {
                return Content("参数错误！");
            }

            postModel.Token = WeixinManager.Token;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = WeixinManager.EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = WeixinManager.AppId;//根据自己后台的设置保持一致

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            var logPath = Server.MapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(Request.InputStream, postModel, maxRecordCount);


            try
            {
                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName)));
                if (messageHandler.UsingEcryptMessage)
                {
                    messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName)));
                }

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;


                //执行微信处理过程
                messageHandler.Execute();

                //测试时可开启，帮助跟踪数据

                //if (messageHandler.ResponseDocument == null)
                //{
                //    throw new Exception(messageHandler.RequestDocument.ToString());
                //}

                if (messageHandler.ResponseDocument != null)
                {
                    messageHandler.ResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName)));
                }

                if (messageHandler.UsingEcryptMessage)
                {
                    //记录加密后的响应信息
                    messageHandler.FinalResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_Final_{1}.txt", _getRandomFileName(), messageHandler.RequestMessage.FromUserName)));
                }

                //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
                //return new WeixinResult(messageHandler);//v0.8+
            }
            catch (Exception ex)
            {
                using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + _getRandomFileName() + ".txt")))
                {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);
                    //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                    if (messageHandler.ResponseDocument != null)
                    {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }

                    if (ex.InnerException != null)
                    {
                        tw.WriteLine("========= InnerException =========");
                        tw.WriteLine(ex.InnerException.Message);
                        tw.WriteLine(ex.InnerException.Source);
                        tw.WriteLine(ex.InnerException.StackTrace);
                    }

                    tw.Flush();
                    tw.Close();
                }
                return Content("");
            }
        }

        #region 权限验证

        /// <summary>
        /// 静默授权，仅返回用户的openId
        /// </summary>
        /// <returns></returns>
        public RedirectResult OAuth2ForBase(string redirectUrl)
        {
            var url = OAuthApi.GetAuthorizeUrl(WeixinManager.AppId,
                "http://fengrui358.vicp.cc/TimeGallery/Weixin/GetWeixinUserInfo", redirectUrl, OAuthScope.snsapi_base);

            return Redirect(url);
        }

        /// <summary>
        /// 带警告页面的授权，返回用户的详细信息
        /// </summary>
        /// <returns></returns>
        public RedirectResult OAuth2ForUserInfo(string redirectUrl)
        {
            var url = OAuthApi.GetAuthorizeUrl(WeixinManager.AppId,
                "http://fengrui358.vicp.cc/TimeGallery/Weixin/GetWeixinUserInfo", redirectUrl, OAuthScope.snsapi_userinfo);

            return Redirect(url);
        }

        /// <summary>
        /// 网页授权回调页面
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state">内部实际上传的是回调Url</param>
        /// <returns></returns>
        public ActionResult GetWeixinUserInfo(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                //todo：请关注微信公众号获取授权
                return Content("您拒绝了授权！");
            }

            if (!state.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                //todo：请关注微信公众号获取授权
                return Content("验证失败！请从正规途径进入！");
            }

            //通过，用code换取access_token
            var result = OAuthApi.GetAccessToken(WeixinManager.AppId, WeixinManager.WeixinAppSecret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                LogManager.GetCurrentClassLogger().Error(result.errmsg);
                return Content("错误：" + result.errmsg);
            }

            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            //Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            //Session["OAuthAccessToken"] = result;

            //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
            //OAuthUserInfo userInfo = null;

            WeixinUserInfoResult weixinUserInfo;

            try
            {
                //已关注，可以得到详细信息
                //userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);

                weixinUserInfo = CommonApi.GetUserInfo(WeixinManager.AppId, result.openid);
                _userManager.TryUpdateUserInfo((UserModel) weixinUserInfo);
                
                return Redirect(state);
            }
            catch (ErrorJsonResultException ex)
            {
                //未关注，只能授权，无法得到详细信息
                //这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"
                //return Content("用户已授权，授权Token：" + result);

                LogManager.GetCurrentClassLogger().Error(ex);

                return Content("内部错误");
            }
        }

        #endregion
    }
}