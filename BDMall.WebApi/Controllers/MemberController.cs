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
        ///  查询会员列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        [ProducesResponseType(typeof(PageData<MemberDto>), 200)]
        public SystemResult Search([FromBody] MbrSearchCond condition)
        {
            var result = new SystemResult() { Succeeded =true };

            var testUser = CurrentUser;

            result.ReturnValue = memberBll.SearchMember(condition);
            return result;
        }

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost("CreateMember")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public SystemResult CreateMember([FromBody]RegisterMember member)
        {           
            var result = memberBll.CreateMember(member);
            var cur = this.CurrentUser;
            return result;
        }
    }
}
