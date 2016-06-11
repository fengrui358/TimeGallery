﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace TimeGallery.Filters
{
    public class TrackPageLoadPerformanceAttribute : ActionFilterAttribute
    {

        //创建字典来记录开始时间，key是访问的线程Id.
        private readonly Dictionary<int, DateTime> _start = new Dictionary<int, DateTime>();

        //创建字典来记录当前访问的页面Url.
        private readonly Dictionary<int, string> _url = new Dictionary<int, string>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //过滤掉ChildAction, 因为ChildAction实际上不是一个单独的页面
            if (filterContext.IsChildAction) return;

            var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            try
            {
                _start.Add(currentThreadId, DateTime.Now);
                _url.Add(currentThreadId, filterContext.HttpContext.Request.Url == null
                    ? string.Empty
                    : filterContext.HttpContext.Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if (!_start.ContainsKey(currentThreadId)) return;

            try
            {

                //计算出当前页面访问耗时
                var costSeconds = (DateTime.Now - _start[currentThreadId]).TotalSeconds;
                if (costSeconds > 2) //如果耗时超过2秒，就是用log4net打印出，具体是哪个页面访问超过了2秒，具体使用了多长时间。
                {
                    LogManager.GetCurrentClassLogger()
                        .Info(
                            $"Access the action more than 2 seconds. cost seconds {_url[currentThreadId]}.  URL: {costSeconds}");
                }
                else
                {
                    //todo:优化描述
                    LogManager.GetCurrentClassLogger()
                        .Trace(
                            $"Access the action. cost seconds {_url[currentThreadId]}.  URL: {costSeconds}");
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
            finally
            {
                _start.Remove(currentThreadId);
                _url.Remove(currentThreadId);
            }
        }
    }
}