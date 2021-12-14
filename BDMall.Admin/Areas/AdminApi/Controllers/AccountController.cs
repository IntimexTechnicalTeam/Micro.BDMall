using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Framework;
using Web.Jwt;
using Web.Mvc;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]/[action]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        public AccountController(IComponentContext services) : base(services)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]      
        public async Task<SystemResult> LoginByAPI([FromForm] LoginInput input)
        {
            var user = await loginBLL.CheckAdminLogin(input);
            var result = await loginBLL.AdminLogin(user);
            if (result.Succeeded)
            {
                var userInfo = result.ReturnValue as UserDto;

                var claimList = new List<Claim>() { };
                claimList.Add(new Claim("UserId", $"{userInfo.Id }"));
                claimList.Add(new Claim("Lang", userInfo.Language.ToString()));      
                claimList.Add(new Claim("CurrencyCode", "HKD"));                     //为了兼容默认一个
                claimList.Add(new Claim("Account", $"{userInfo.Account}"));
                claimList.Add(new Claim("LoginType", $"{ userInfo.LoginType.ToInt() }"));

                userInfo.Token = jwtToken.CreateToken(claimList);

                //把登录信息：token,权限，菜单，角色放进redis中
                string key = $"{CacheKey.CurrentUser}";
                await RedisHelper.HSetAsync(key, userInfo.Id.ToString(), userInfo);

                result.Succeeded = true;
                result.ReturnValue = userInfo.Token;
                HttpContext.Response.Cookies.Append("access_token", userInfo.Token);
            }

            return result;
        }

        /// <summary>
        /// 修改语言
        /// </summary>
        /// <param name="Lang"></param>
        /// <returns></returns>       
        [HttpGet]      
        public async Task<SystemResult> ChangeLang(Language Lang)
        {
            var result = new SystemResult() { Succeeded = true };

            //根据header中的token和Lang参数，重新生成token,返回token

            return result;
        }

    }
}
