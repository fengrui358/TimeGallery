using System.Web.Mvc;
using Autofac;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using TimeGallery.Consts;
using TimeGallery.Controllers;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Weixin;

namespace TimeGallery.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 是否需要校验
        /// </summary>
        private readonly bool _isAuth;

        public AuthFilter(bool isAuth = true)
        {
            _isAuth = isAuth;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if DEBUG
            //todo:测试代码，替换账号
            //使用fengrui的微信号作为测试账号，openId="oIKlFw0yLVagA1nNfEegqP_2o6Bs"
            //var sessionId = IocHelper.Container.Resolve<ISessionManager>().AddSession("oIKlFw0yLVagA1nNfEegqP_2o6Bs");
            //((Controller) filterContext.Controller).Session[SystemString.SessionKey] = sessionId;
#endif

            if (_isAuth)
            {
                var controller = filterContext.Controller as Controller;
                if (controller != null)
                {
                    var isVaildSession = IocHelper.Container.Resolve<ISessionManager>().VerifySession(controller.HttpContext);
                    if (!isVaildSession)
                    {
                        if (controller.Request.Url != null)
                        {
                            var redirectUrl = controller.Request.Url.ToString();

                            var url = OAuthApi.GetAuthorizeUrl(WeixinManager.AppId,
                                SimpleUrlHelper.GenerateUrl(nameof(WeixinController), nameof(WeixinController.GetWeixinUserInfo)),
                                redirectUrl,
                                OAuthScope.snsapi_userinfo);

                            filterContext.Result = new RedirectResult(url);
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}