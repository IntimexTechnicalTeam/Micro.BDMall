using Autofac;
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

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        IJwtToken jwtToken;
        public TokenController(IComponentContext service) : base(service)
        {
            jwtToken = this.Services.Resolve<IJwtToken>();
        }

        /// <summary>
        /// TempUser访问时调用这个CreateToken
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("CreateToken")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> CreateToken()
        {
            string ticket = jwtToken.CreateDefautToken();
            var result = new SystemResult() { Succeeded = true };
            result.ReturnValue = ticket;
            return result;
        }

        /// <summary>
        /// 当用户接到401状态码时(token过期)调用这个RefreshToken
        /// </summary>
        [AllowAnonymous]
        [HttpGet("RefreshToken")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> RefreshToken(string Token)
        { 
            string ticket = jwtToken.RefreshToken(Token);
            var result = new SystemResult() { Succeeded = true };

            result.ReturnValue = ticket;
            return result;
        }
    }
}
