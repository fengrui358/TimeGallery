using System;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using TimeGallery.Consts;
using TimeGallery.Controllers;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Managers;
using TimeGallery.Weixin;

namespace TimeGallery.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 是否需要校验
        /// </summary>
        private readonly AuthFilterTypeDefine _authFilterType;

        public AuthFilter(AuthFilterTypeDefine authFilterType = AuthFilterTypeDefine.Must)
        {
            _authFilterType = authFilterType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if DEBUG
            //todo:测试代码，替换账号
            //使用fengrui的微信号作为测试账号，openId="oIKlFw0yLVagA1nNfEegqP_2o6Bs"
            var sessionId = IocHelper.Container.Resolve<ISessionManager>().AddSession("oIKlFw0yLVagA1nNfEegqP_2o6Bs");
            ((Controller)filterContext.Controller).Session[SystemString.SessionKey] = sessionId;
#endif

            if (_authFilterType == AuthFilterTypeDefine.Must)
            {
                var controller = filterContext.Controller as Controller;
                if (controller != null)
                {
                    //如果在Session中发现一个空Session则证明之前已经校验过了此处不再进行校验，直接跳转报错
                    if (IocHelper.Container.Resolve<ISessionManager>().GetSessionFromCookie(controller.HttpContext) ==
                        SessionManager.TempUserSession)
                    {
                        //todo:直接跳转报错
                        throw new Exception("没有权限访问");
                    }

                    VerifyAndRedirect(controller.HttpContext);
                }
            }

            if (_authFilterType == AuthFilterTypeDefine.Try)
            {
                var controller = filterContext.Controller as Controller;
                if (controller?.HttpContext.Request.UserAgent != null &&
                    controller.HttpContext.Request.UserAgent.IndexOf(SystemString.WeixinBrowserUserAgentKeyWork, StringComparison.OrdinalIgnoreCase) !=
                    -1)
                {
                    //如果在Session中发现一个空Session则证明之前已经校验过了此处不再进行校验，直接放行
                    if (!(IocHelper.Container.Resolve<ISessionManager>().GetSessionFromCookie(controller.HttpContext) ==
                          SessionManager.TempUserSession))
                    {
                        VerifyAndRedirect(controller.HttpContext);
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        public static void VerifyAndRedirect(HttpContextBase httpContext)
        {
            var isVaildSession = IocHelper.Container.Resolve<ISessionManager>().VerifySession(httpContext);
            if (!isVaildSession)
            {
                if (httpContext.Request.Url != null)
                {
                    var redirectUrl = httpContext.Request.Url.ToString();

                    var url = OAuthApi.GetAuthorizeUrl(WeixinManager.AppId,
                        SimpleUrlHelper.GenerateUrl(nameof(WeixinController), nameof(WeixinController.GetWeixinUserInfo)),
                        redirectUrl,
                        OAuthScope.snsapi_base);

                    httpContext.Response.Redirect(url);
                }
            }
        }
    }

    public enum AuthFilterTypeDefine
    {
        /// <summary>
        /// 必须校验
        /// </summary>
        Must,

        /// <summary>
        /// 微信客户端访问时可尝试校验，如果非微信客户端访问时不校验
        /// </summary>
        Try,

        /// <summary>
        /// 不需要校验
        /// </summary>
        Not,
    }
}