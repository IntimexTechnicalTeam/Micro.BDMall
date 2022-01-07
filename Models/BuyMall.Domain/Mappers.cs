using AutoMapper;
using BDMall.Model;
using BDMall.Domain;
using Web.Framework;

namespace Domain
{
    /// <summary>
    /// 自动映射，因为继承ICreateMapper，AutoMapperConfiguration类会自动找到这个类来进行自动CreateMap,省去自己手动去CreateMap
    /// </summary>
    public class Mappers : Profile, ICreateMapper
    {
        public Mappers()
        {            
            ConfigMap<Member, MemberDto>();
            ConfigMap<User, UserDto>();
            ConfigMap<Role, RoleDto>();
            ConfigMap<Permission, PermissionDto>();
            ConfigMap<ProductCatalog, ProductCatalogDto>();
            ConfigMap<ProductAttributeValue, ProductAttributeValueDto>();
            ConfigMap<ProductCatalogSummaryView, ProductCatalogDto>();
            ConfigMap<ProductCatalogDto, ProductCatalogEditModel>();
            ConfigMap<MerchantView, Merchant>();
            ConfigMap<MerchantECShipInfo, MerchantECShipInfoDto>();
            ConfigMap<MerchantActiveShipMethod, MerchantActiveShipMethodDto>();
            ConfigMap<CodeMasterDto, CodeMaster>();
            ConfigMap<ExpressCompanyDto, ExpressCompany>();
            ConfigMap<CurrencyExchangeRateDto, CurrencyExchangeRate>();
            ConfigMap<CustomMenuDto, CustomMenu>();
            ConfigMap<CustomMenuDetailDto, CustomMenuDetail>();

            ConfigMap<EmailTempItemDto, EmailTempItem>();
            ConfigMap<EmailTemplateDto, EmailTemplate>();
            ConfigMap<EmailTypeTempItemDto, EmailTypeTempItem>();



            ConfigMap<ProductAttributeDto, ProductAttribute>();
        }
        void ConfigMap<TSource, TDestination>()
        {
            CreateMap<TSource, TDestination>();
            CreateMap<TDestination, TSource>();
        }
    }

    public static class MapperExtension
    {
        /// <summary>
        /// 配置一次就可以實現兩個實體之間的相互映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="action"></param>
        public static void ConfigMap<TSource, TDestination>(this IMapperConfigurationExpression action)
        {
            action.CreateMap<TSource, TDestination>();
            action.CreateMap<TDestination, TSource>();
        }
    }
}
