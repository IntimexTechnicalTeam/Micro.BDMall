using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Framework;
using Web.Jwt;

namespace Web.Mvc
{
    public class UserAuthorizeAttribute  : ActionFilterAttribute
    {
        //不能打开构造函数，否则全局注入UserAuthorizeAttribute后，会抛异常
        //public UserAuthorizeAttribute(bool isLogin)
        //{
        //    IsLogin = isLogin;
        //}

        /// <summary>
        /// 是否登录检查标识
        /// </summary>
        public bool IsLogin { get; set; } 

        /// <summary>
        /// 鉴权
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (await BaseAuthority.CheckTokenAuthorize(context, next, IsLogin))
            {
                await next();
            }          
        }
    }
}
