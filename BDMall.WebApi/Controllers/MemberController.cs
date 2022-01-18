using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseApiController
    {
        IMemberBLL memberBll;

        public MemberController(IComponentContext service) : base(service)
        {
            memberBll = this.Services.Resolve<IMemberBLL>();
        }

       

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public SystemResult Register([FromBody]RegisterMember member)
        {           
            var result = memberBll.Register(member);           
            return result;
        }
    }
}
