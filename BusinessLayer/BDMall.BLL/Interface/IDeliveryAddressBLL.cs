namespace BDMall.BLL
{
    public interface IDeliveryAddressBLL : IDependency
    {
        SystemResult CreateAddress(DeliveryAddressDto deliveryInfo);

        SystemResult UpdateAddress(DeliveryAddressDto deliveryInfo);

        SystemResult RemoveAddress(Guid id);
        DeliveryAddressDto GetAddress(Guid id);
        List<DeliveryAddressDto> GetMemberAddress(Guid memberId);
        /// <summary>
        /// 獲取會員香港本地的地址清單
        /// </summary>
        /// <param name="memberId">會員ID</param>
        /// <returns></returns>
        List<DeliveryAddressDto> GetMemberHKAddress(Guid memberId);
        /// <summary>
        /// 獲取會員海外的地址清單
        /// </summary>
        /// <param name="memberId">會員ID</param>
        List<DeliveryAddressDto> GetMemberOverseasAddress(Guid memberId);

        DeliveryAddressDto GetDefaultAddr(Guid memberId);


        List<CountryDto> GetCountries();

        CountryDto GetCountry(int id);

        ProvinceDto GetProvince(int id);

        List<ProvinceDto> GetProvinces(int countryId);

        List<CityDto> GetCities(int provinceId);

        /// <summary>
        /// 送貨地址資料數據校驗
        /// </summary>
        /// <param name="deliveryInfo">地址資料</param>
        SystemResult DeliveryAddressVerification(DeliveryAddressDto deliveryInfo);
    }
}
