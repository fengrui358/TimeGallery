using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using TimeGallery.Helper;
using TimeGallery.Interfaces;
using TimeGallery.Weixin;

namespace TimeGallery.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if Release
            var controller = filterContext.Controller as Controller;
            if (controller != null)
            {
                var isVaildSession = await IocHelper.Container.Resolve<ISessionManager>().VerifySession(controller);
                if (!isVaildSession)
                {
                    if (controller.Request.Url != null)
                    {
                        var redirectUrl = controller.Request.Url.ToString();

                        var url = OAuthApi.GetAuthorizeUrl(WeixinManager.AppId,
                            "http://fengrui358.vicp.cc/TimeGallery/Weixin/OAuth2ForBase", redirectUrl,
                            OAuthScope.snsapi_base);

                        controller.Response.Redirect(url);
                    }
                }
            }
#endif

            base.OnActionExecuting(filterContext);
        }
    }
}