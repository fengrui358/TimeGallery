using System.Web.Mvc;
using Autofac;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
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
                                SimpleUrlHelper.GenerateUrl(nameof(WeixinController), nameof(WeixinController.OAuth2ForBase)),
                                redirectUrl,
                                OAuthScope.snsapi_base);

                            filterContext.Result = new RedirectResult(url);
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}