﻿using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : BaseApiController
    {
        IMerchantBLL merchantBLL;

        public MerchantController(IComponentContext services) : base(services)
        {
            merchantBLL = Services.Resolve<IMerchantBLL>();
        }

        /// <summary>
        /// 获取商家明细信息
        /// </summary>
        /// <param name="merchID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetMerchantInfo")]
        [ProducesResponseType(typeof(SystemResult<MerchantInfoView>), 200)]
        public async Task<SystemResult<MerchantInfoView>> GetMerchantInfo(string merchID)
        {
            var result = new SystemResult<MerchantInfoView>() { Succeeded = true };

            if (merchID.IsEmpty()) throw new BLException();

            result.ReturnValue=await merchantBLL.GetMerchantInfoAsync(Guid.Parse(merchID));
            return result;
        }

        /// <summary>
        /// 获取商家所属商品
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetProducts")]
        [ProducesResponseType(typeof(SystemResult<PageData<MicroProduct>>), 200)]
        public async Task<SystemResult<PageData<MicroProduct>>> GetProducts([FromBody] ProductCond cond)
        {
            var result = new SystemResult<PageData<MicroProduct>>() { Succeeded = true };

            if (cond ==null || cond.MerchantId == Guid.Empty) throw new BLException();

            result.ReturnValue = await merchantBLL.GetMchProductListAsync(cond);
            return result;
        }
    }
}