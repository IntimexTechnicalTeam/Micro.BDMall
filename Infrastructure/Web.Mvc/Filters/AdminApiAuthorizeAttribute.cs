using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Jwt;

namespace Web.Mvc
{
    /// <summary>
    /// Admin 后台，Api请求鉴权，请使用这个Filter
    /// </summary>
    public class AdminApiAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 需要模塊權限 
        /// </summary>
        public string Module;

        /// <summary>
        /// 需要模塊權限
        /// </summary>
        public string[] Modules { get; set; }
        /// <summary>
        /// 需要的功能權限
        /// </summary>
        public string[] Function;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (await BaseAuthority.CheckTokenAuthorize(context, next))
            {
                var flag = await this.CheckActionAsync(context);
                if (!flag)
                {
                    if (IsAjaxRequest(context.HttpContext.Request)) // AJAX请求，返回status标识未登录
                    {
                        context.Result = new JsonResult(new SystemResult { Succeeded = false, Message = "未授权访问" });
                        context.HttpContext.Response.StatusCode = 403;
                        return;
                    }
                    else
                    {
                        context.Result = new JsonResult(new SystemResult { Succeeded = false, Message = "未授权访问" });
                        context.HttpContext.Response.StatusCode = 403;
                        return;
                    }
                }

                await next();
            }
        }

        /// <summary>
        /// 检查用户权限，有权限返回true
        /// </summary>
        /// <returns></returns>
        async Task<bool> CheckActionAsync(ActionExecutingContext context)
        {
            string token = string.Empty;bool flag = false;
            var jwtToken = context.HttpContext.RequestServices.GetService(typeof(IJwtToken)) as IJwtToken;

            var authorization = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring("Bearer ".Length).Trim();
            }

            var payload = jwtToken.DecodeJwt(token);
            var uid = payload["UserId"];
            
            string key = $"{CacheKey.CurrentUser}";
            var cacheUser = await RedisHelper.HGetAsync<UserDto>(key, uid);

            flag = CheckRolePermission(cacheUser.Roles);
            return flag;
        }

        private bool CheckRolePermission(List<RoleDto> roles)
        {
            try
            {
                foreach (var role in roles)
                {
                    if (Function?.Length > 0)
                    {
                        if (Modules?.Length > 0)
                        {
                            foreach (var Module in Modules)
                            {
                                //检查功能权限
                                if (role.PermissionList.Any(d => d.Module == Module && Function.Contains(d.Function)))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            //检查功能权限
                            if (role.PermissionList.Any(d => d.Module == Module && Function.Contains(d.Function)))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Module) && Modules?.Length < 1)
                        {
                            return true;
                        }

                        if (Modules?.Length > 0)
                        {
                            foreach (var Module in Modules)
                            {
                                if (role.PermissionList.Any(d => d.Module == Module))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            //目前权限控制级别为模块，所以只判断是否有模块的权限
                            if (role.PermissionList.Any(d => d.Module == Module))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
        }


        /// <summary>
        ///  是否AJAX请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsAjaxRequest(HttpRequest request)
        {
            bool result = false;
            var requestValue = request.Headers.Where(c => c.Key.ToLower() == "x-requested-with").FirstOrDefault().Value;
            if (requestValue.Count > 0 && requestValue.First<string>().ToLower() == "xmlhttprequest")
            {
                result = true;
            }
            //IsAjaxRequest()是判断header里面有没有X-Requested-With字段以及它是否是XMLHttpRequest
            return result;
        }
    }
}
