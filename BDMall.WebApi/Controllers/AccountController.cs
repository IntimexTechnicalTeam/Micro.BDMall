using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Framework;
using Web.Jwt;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        
        public AccountController(IComponentContext service) : base(service)
        { 
        }

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<SystemResult> Login([FromBody] LoginInput input)
        {
            var result = await loginBLL.Login(input);

            if (result.Succeeded)
            {
                var userInfo = result.ReturnValue as MemberDto;

                var claimList = new List<Claim>() {  };
                claimList.Add(new Claim("UserId",userInfo.Id.ToString()));
                claimList.Add(new Claim("Lang", userInfo.Language.ToString()));
                claimList.Add(new Claim("CurrencyCode",userInfo.CurrencyCode));
                claimList.Add(new Claim("Account", userInfo.Account));
                claimList.Add(new Claim("LoginType", $"{ LoginType.Member.ToInt() }"));
                string ticket = jwtToken.CreateToken(claimList);

                result.Succeeded = true;
                result.ReturnValue = ticket;
            }

            return result;
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Logout")]
        public async Task<SystemResult> Logout()
        {
            string ticket = jwtToken.CreateDefautToken();
            var result = new SystemResult() { Succeeded = true };
            result.ReturnValue = ticket;
            return result;
        }

        /// <summary>
        /// 修改语言
        /// </summary>
        /// <param name="Lang"></param>
        /// <returns></returns>       
        [HttpGet("ChangeLang")]
        public async Task<SystemResult> ChangeLang(Language Lang)
        {
            var result = new SystemResult() { Succeeded = true };           

             //根据header中的token和Lang参数，重新生成token,返回token

            return result;
        }

        /// <summary>
        /// 修改币种
        /// </summary>
        /// <param name="CurrencyCode"></param>
        /// <returns></returns>       
        [HttpGet("ChangeCurrencyCode")]
        public async Task<SystemResult> ChangeCurrencyCode(string CurrencyCode)
        {
            var result = new SystemResult() { Succeeded = true };

            //根据header中的token和CurrencyCode参数，重新生成token,返回token

            return result;
        }

    }
}
