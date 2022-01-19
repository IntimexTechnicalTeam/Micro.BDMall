﻿using Autofac;
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
        IUserBLL userBLL;
        public AccountController(IComponentContext services) : base(services)
        {
            userBLL = Services.Resolve<IUserBLL>();
        }

        /// <summary>
        /// 后台用户登录
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
                claimList.Add(new Claim("IsLogin", "true"));
                claimList.Add(new Claim("LoginType", $"{ userInfo.LoginType }"));
                claimList.Add(new Claim("Email", $"{ userInfo.Email }"));


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
            result.ReturnValue = await userBLL.ChangeLang(CurrentUser, Lang);
            return result;
        }

    }
}
