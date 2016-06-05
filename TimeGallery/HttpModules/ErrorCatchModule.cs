using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using TimeGallery.Models.Javascript;

namespace TimeGallery.HttpModules
{
    public class ErrorCatchModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(context_Error);

        }

        public void Dispose()
        {            
        }

        private void context_Error(object sender, EventArgs e)
        {
            //此处处理异常
            HttpContext ctx = HttpContext.Current;
            HttpResponse response = ctx.Response;
            HttpRequest request = ctx.Request;

            //获取到HttpUnhandledException异常，这个异常包含一个实际出现的异常
            Exception ex = ctx.Server.GetLastError();
            //实际发生的异常
            Exception iex = ex.InnerException;

            //todo：重定向错误            
            //response.Write("来自ErrorModule的错误处理<br />");
            //response.Write(iex.Message);

            LogManager.GetCurrentClassLogger().Error(ex);
            response.Write(new RequestResult(RequestResultTypeDefine.Error, "系统内部错误"));

            ctx.Server.ClearError();
        }
    }
}