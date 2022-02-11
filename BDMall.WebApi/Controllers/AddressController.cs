using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Framework;
using Web.Mvc;

namespace BDMall.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : BaseApiController
    {
        public IDeliveryAddressBLL deliveryAddressBLL;

        public AddressController(IComponentContext services) : base(services)
        {
            deliveryAddressBLL = Services.Resolve<IDeliveryAddressBLL>();
        }

        /// <summary>
        /// 获取会员收货地址
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetAddresses")]
        [ProducesResponseType(typeof(SystemResult<List<DeliveryAddressDto>>), 200)]
        public SystemResult<List<DeliveryAddressDto>> GetAddresses()
        {
            var result = new SystemResult<List<DeliveryAddressDto>>();
            result.ReturnValue = deliveryAddressBLL.GetMemberAddress(Guid.Parse(CurrentUser.UserId));
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 获取国家数据
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Country")]
        [ProducesResponseType(typeof(SystemResult<List<CountryDto>>), 200)]
        public SystemResult<List<CountryDto>> Country()
        {
            var result = new SystemResult<List<CountryDto>>();
            result.ReturnValue = deliveryAddressBLL.GetCountries();
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 获取省份数据
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Province")]
        [ProducesResponseType(typeof(SystemResult<List<ProvinceDto>>), 200)]
        public SystemResult<List<ProvinceDto>> Province(int countryId)
        {
            var result = new SystemResult<List<ProvinceDto>>();
            result.ReturnValue = deliveryAddressBLL.GetProvinces(countryId);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 获取城市数据
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("City")]
        [ProducesResponseType(typeof(SystemResult), 200)]      
        public SystemResult City(int provinceId)
        {
            var result = new SystemResult<List<CityDto>>();
            result.ReturnValue = deliveryAddressBLL.GetCities(provinceId);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 新增送货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public SystemResult Add([FromBody] DeliveryAddressDto address)
        {
            SystemResult result = new SystemResult();
            result = deliveryAddressBLL.CreateAddress(address);
            return result;
        }

        /// <summary>
        /// 修改送货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public SystemResult Update([FromBody] DeliveryAddressDto address)
        {
            SystemResult result = new SystemResult();
            result = deliveryAddressBLL.UpdateAddress(address);
            return result;
        }

        /// <summary>
        /// 删除送货地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Remove")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public SystemResult Remove(Guid id)
        {
            SystemResult result = new SystemResult();
            result = deliveryAddressBLL.RemoveAddress(id);
            return result;
        }
    }
}
