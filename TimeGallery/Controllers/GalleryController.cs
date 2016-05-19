using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Serialization;
using Autofac;
using NLog;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using TimeGallery.DataBase;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Models;
using TimeGallery.Weixin;

namespace TimeGallery.Controllers
{
    public class GalleryController : Controller
    {
        private IConfigurationManager _configurationManager;

        public GalleryController(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        // GET: Gallery
        public ActionResult Index()
        {
            ViewBag.Title = _configurationManager.GetAppSetting("WebTitle");
            var contents = StorageHelper.Search(DateTime.Now);

            var result = from content in contents
                let t = content.CreateTime
                group content by new DateTime(t.Year, t.Month, t.Day)
                into g
                select new ContentWrapperModel(g.Key, g);

            return View(result);
        }

        public Task<ActionResult> Upload(string code, string state)
        {
            return Task.Factory.StartNew<ActionResult>(() =>
            {
                if (string.IsNullOrEmpty(code))
                {
                    //todo：请关注微信公众号获取授权
                    return Content("您拒绝了授权！");
                }

                if (state != "upload")
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
                    return Content("错误：" + result.errmsg);
                }

                //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
                //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
                Session["OAuthAccessTokenStartTime"] = DateTime.Now;
                Session["OAuthAccessToken"] = result;

                //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
                OAuthUserInfo userInfo = null;
                ViewBag.UpToken = GetQiniuToken();

                try
                {
                    //已关注，可以得到详细信息
                    userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                    ViewBag.UserInfo = userInfo;
                }
                catch (ErrorJsonResultException ex)
                {
                    //未关注，只能授权，无法得到详细信息
                    //这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"
                    //return Content("用户已授权，授权Token：" + result);

                    LogManager.GetCurrentClassLogger().Error(ex);
                }

                return View();
            });
        }

        //[HttpPost]
        public ActionResult GetQiniuToken()
        {
            var upToken = QiniuHelper.GetToken();

            return Json(new {uptoken = upToken}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加类容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddContent(AddContentModel addContentModel)
        {
            var result = StorageHelper.InsertContent(addContentModel);

            return Json(new {success = result});
        }

        #region 权限验证

        /// <summary>
        /// 静默授权，仅返回用户的openId
        /// </summary>
        /// <returns></returns>
        public RedirectResult OAuth2ForBaseUpload()
        {
            var url = OAuthApi.GetAuthorizeUrl(WeixinManager.AppId,
                "http://fengrui358.vicp.cc/TimeGallery/Gallery/Upload", "upload", OAuthScope.snsapi_base);

            return Redirect(url);
        }

        /// <summary>
        /// 带警告页面的授权，返回用户的详细信息
        /// </summary>
        /// <returns></returns>
        public ActionResult OAuth2ForUserInfoUpload()
        {
            var url = OAuthApi.GetAuthorizeUrl(WeixinManager.AppId,
                "http://fengrui358.vicp.cc/TimeGallery/Gallery/Upload", "upload", OAuthScope.snsapi_userinfo);
            return Redirect(url);
        }

        #endregion
    }
}