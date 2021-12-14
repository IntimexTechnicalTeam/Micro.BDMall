using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Web.Framework;

namespace Web.Mvc
{
    public class LogActionFilter : ActionFilterAttribute, IActionFilter
    {
        string key = "action";
        ILogger _logger;

        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            this._logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            key = Guid.NewGuid().ToString();
            context.HttpContext.Items.Add(key, stopWatch);

            var attributes = context.ActionDescriptor.FilterDescriptors;
            bool isAnonymous = attributes.Any(p => p.Filter is AllowAnonymousFilter);//匿名标识 无需验证
            if (!isAnonymous)
            {
                string controllerName = context.ActionDescriptor.RouteValues["controller"].ToString().ToLower();
                string actionName = context.ActionDescriptor.RouteValues["action"].ToString().ToLower();
                IDictionary<string, object> paramDic = context.ActionArguments;
                string paramResult = JsonUtil.ObjectToJson(paramDic);

                this._logger.LogTrace(string.Format(@"[{0}/{1} 请求参数 {2}]", controllerName, actionName, paramResult));

                //APIResult result = new APIResult();
                //var ip = context.HttpContext.Request.GetUserIP();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var stopWatch = context.HttpContext.Items[key] as Stopwatch;
            if (stopWatch != null)
            {
                stopWatch.Stop();
                string controllerName = context.ActionDescriptor.RouteValues["controller"].ToString().ToLower();
                string actionName = context.ActionDescriptor.RouteValues["action"].ToString().ToLower();
                this._logger.LogDebug(string.Format(@"[{0}/{1} 用时 {2}ms]", controllerName, actionName, stopWatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}
