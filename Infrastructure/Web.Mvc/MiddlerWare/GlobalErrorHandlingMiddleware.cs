using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Web.Framework;

namespace Web.Mvc
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    public class GlobalErrorHandlingMiddleware
    {
        readonly RequestDelegate next;
        readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this._logger = logger;
        }
        
       async Task Response(HttpContext context, SystemResult result)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                SystemResult result = new SystemResult();

                if (ex is InvalidInputException || ex is ServiceException)
                {
                    result.Succeeded = false;
                    result.Message = ex.Message;

                    await this.Response(context, result);
                    return;
                }

                if (ex is ApiServiceException)
                {
                    var ApiServiceException = ex as ApiServiceException;
                    result.Succeeded = false;
                    result.Message = ex.Message;

                    await this.Response(context, result);
                    return;
                }

                if (ex is BLException)
                {
                    result.Succeeded = false;
                    result.Message = ex.Message;

                    await this.Response(context, result);
                    return;
                }

                string error = "\r\n 异常类型：" + ex.GetType().FullName + "\r\n 异常源：" + ex.Source + "\r\n 异常位置=" + ex.TargetSite + " \r\n 异常信息=" + ex.Message + " \r\n 异常堆栈：" + ex.StackTrace;
                this._logger.LogError(error);

                result.Succeeded = false;
                result.Message = "服务器内部错误";

                if (OutputException)
                {
                    result.Message = $"{ex.Message}\r\n{ex.StackTrace}"; ;
                }

#if DEBUG
                throw new Exception(ex.Message, ex);
#endif

                await this.Response(context, result);
                return;
            }
        }

        public static bool OutputException
        {
            get
            {
                return Globals.Configuration["OutputException"] == "1";
            }
        }
    }
}
