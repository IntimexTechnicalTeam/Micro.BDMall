

namespace BDMall.BLL
{
    public class DeliveryAddressBLL : BaseBLL, IDeliveryAddressBLL
    {
        IDeliveryAddressRepository _deliveryAddressRepository;
        public DeliveryAddressBLL(IServiceProvider services) : base(services)
        {
            _deliveryAddressRepository = Services.Resolve<IDeliveryAddressRepository>();
        }
        public SystemResult CreateAddress(DeliveryAddressDto deliveryInfo)
        {
            SystemResult result = new SystemResult();


            if (string.IsNullOrEmpty(deliveryInfo.Email))
            {
                deliveryInfo.Email = CurrentUser.Email;
            }
            if (string.IsNullOrEmpty(deliveryInfo.Mobile))
            {
                deliveryInfo.Mobile = deliveryInfo.Phone;
            }

            result = DeliveryAddressVerification(deliveryInfo);
            if (!result.Succeeded)
            {
                return result;
            }

            ////清除省份對象
            //if (deliveryInfo.Province != null)
            //{ 
            //    deliveryInfo.Province = null;
            //}

            deliveryInfo.Id = Guid.NewGuid();
            deliveryInfo.MemberId = Guid.Parse(CurrentUser.UserId);

            deliveryInfo.CreateBy = Guid.Parse(CurrentUser.UserId);
            deliveryInfo.CreateDate = DateTime.Now;
            deliveryInfo.IsActive = true;
            deliveryInfo.IsDeleted = false;

            if (deliveryInfo.Default)
            {
                _deliveryAddressRepository.UpdateOtherAddressNotDefault(Guid.Parse(CurrentUser.UserId));
            }
            var dbMobel = AutoMapperExt.MapTo<DeliveryAddress>(deliveryInfo);
            baseRepository.Insert(dbMobel);

            result.ReturnValue = dbMobel;
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.SaveSuccess;

            return result;
        }

        public DeliveryAddressDto GetAddress(Guid id)
        {
            DeliveryAddressDto result;

            result = _deliveryAddressRepository.GetByKey(id);
            return result;
        }

        public List<DeliveryAddressDto> GetMemberAddress(Guid memberId)
        {
            List<DeliveryAddressDto> result;

            result = _deliveryAddressRepository.SearchAddress(memberId, true, false);

            return result;
        }

        /// <summary>
        /// 獲取會員香港本地的地址清單
        /// </summary>
        /// <param name="memberId">會員ID</param>
        public List<DeliveryAddressDto> GetMemberHKAddress(Guid memberId)
        {
            List<DeliveryAddressDto> result;

            result = _deliveryAddressRepository.SearchLocalAddress(memberId, true, false);

            return result;
        }

        /// <summary>
        /// 獲取會員海外的地址清單
        /// </summary>
        /// <param name="memberId">會員ID</param>
        public List<DeliveryAddressDto> GetMemberOverseasAddress(Guid memberId)
        {
            List<DeliveryAddressDto> result;

            result = _deliveryAddressRepository.SearchOverseasAddress(memberId, true, false);

            return result;
        }

        public List<CityDto> GetCities(int provinceId)
        {
            throw new NotImplementedException();
        }
        private static List<Country> CountryList;
        public List<CountryDto> GetCountries()
        {

            var dtos = new List<CountryDto>();
            if (CountryList == null)
            {
                CountryList = baseRepository.GetList<Country>().Where(d => d.IsActive == true && d.IsDeleted == false).OrderBy(o => o.Seq).ThenBy(t => t.Code).ToList();
                var model = AutoMapperExt.MapTo <List<CountryDto>>(CountryList);
                foreach (var item in model)
                {
                    item.Name = NameUtil.GetCountryName(CurrentUser.Lang.ToString(), item);
                }
            }
            dtos = AutoMapperExt.MapToList<Country, CountryDto>(CountryList);

            return dtos;
        }

        public DeliveryAddressDto GetDefaultAddr(Guid memberId)
        {
            throw new NotImplementedException();
        }

        private static readonly Dictionary<int, List<ProvinceDto>> ProvinceList = new Dictionary<int, List<ProvinceDto>>();
        public List<ProvinceDto> GetProvinces(int countryId)
        {
            List<ProvinceDto> dtoList = null;
            List<Province> list = null;
            if (ProvinceList.Keys.Contains(countryId))
            {
                dtoList = ProvinceList[countryId];
            }

            if (dtoList != null)
            {
                foreach (var item in dtoList)
                {
                    item.Name = NameUtil.GetProviceName(CurrentUser.Lang.ToString(), item);
                }
                return dtoList;
            }
            if (countryId == 0)
            {
                list = baseRepository.GetList<Province>().Where(d => d.IsActive == true && d.IsDeleted == false).ToList();
            }
            else
            {
                list = baseRepository.GetList<Province>().Where(d => d.CountryId == countryId && d.IsActive == true && d.IsDeleted == false).ToList();
            }

            dtoList = AutoMapperExt.MapToList<Province, ProvinceDto>(list);

            foreach (var item in dtoList)
            {
                item.Name = NameUtil.GetProviceName(CurrentUser.Lang.ToString(), item);
            }
            ProvinceList[countryId] = dtoList;
            return dtoList;
        }

        public SystemResult RemoveAddress(Guid id)
        {
            SystemResult result = new SystemResult();

            if (id == Guid.Empty)
            {
                throw new BLException(BDMall.Resources.Message.IdEmpty);
            }
            var data = _deliveryAddressRepository.GetByKey(id);
            if (data == null)
            {
                throw new BLException(BDMall.Resources.Message.RecordExist);
            }
            data.IsDeleted = true;
            _deliveryAddressRepository.Update(data);
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.DeleteSucceeded;

            return result;
        }

        public SystemResult UpdateAddress(DeliveryAddressDto deliveryInfo)
        {
            SystemResult result = new SystemResult();

            if (deliveryInfo.Id == Guid.Empty)
            {
                throw new BLException(BDMall.Resources.Message.IdEmpty);
            }

            if (string.IsNullOrEmpty(deliveryInfo.Email))
            {
                deliveryInfo.Email = CurrentUser.Email;
            }
            if (string.IsNullOrEmpty(deliveryInfo.Mobile))
            {
                deliveryInfo.Mobile = deliveryInfo.Phone;
            }

            result = DeliveryAddressVerification(deliveryInfo);
            if (!result.Succeeded)
            {
                return result;
            }

            deliveryInfo.MemberId = Guid.Parse(CurrentUser.UserId);

            if (deliveryInfo.Default)
            {
                _deliveryAddressRepository.UpdateOtherAddressNotDefault(deliveryInfo.MemberId);
            }

            _deliveryAddressRepository.Update(deliveryInfo);
            result.ReturnValue = deliveryInfo;
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.SaveSuccess;

            return result;
        }

        public CountryDto GetCountry(int id)
        {
            var country = baseRepository.GetModel<Country>(p => p.Id == id);

            var dto = AutoMapperExt.MapTo<CountryDto>(country);

            return dto;
        }

        public ProvinceDto GetProvince(int id)
        {
            var province = baseRepository.GetModel<Province>(p => p.Id == id);

            var dto = AutoMapperExt.MapTo<ProvinceDto>(province);

            return dto;
        }

        /// <summary>
        /// 送貨地址資料數據校驗
        /// </summary>
        /// <param name="deliveryInfo">地址資料</param>
        public SystemResult DeliveryAddressVerification(DeliveryAddressDto deliveryInfo)
        {
            var sysRslt = new SystemResult();

            if (deliveryInfo != null)
            {
                var lang = CurrentUser.Lang;
                string lengthOverFormat = BDMall.Resources.Message.DataLengthOverFlow;

                int maxNameLen = 17;
                //姓氏
                if (string.IsNullOrEmpty(deliveryInfo.LastName))
                {
                    sysRslt.Message = "[" + Resources.Label.AddressLastName + "] " + Resources.Message.FieldRequire;
                    return sysRslt;
                }
                else
                {
                    if (!UTF8StringLengthChecking(deliveryInfo.LastName, maxNameLen))
                    {
                        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.AddressLastName, maxNameLen.ToString());
                        return sysRslt;
                    }
                }

                //姓氏特殊字符檢查
                if (NameUtil.IsExistSpecialCharacter(deliveryInfo.LastName))
                {
                    sysRslt.Message = "[" + Resources.Label.AddressLastName + "] " + Resources.Message.EnterCorrectName;
                    return sysRslt;
                }


                //電話號碼長度
                if (string.IsNullOrEmpty(deliveryInfo.Mobile))
                {
                    sysRslt.Message = "[" + Resources.Label.AddressMobileNum + "] " + Resources.Message.FieldRequire;
                    return sysRslt;
                }
                else
                {
                    if (deliveryInfo.Mobile.Length > 25)
                    {
                        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.AddressMobileNum, "25");
                        return sysRslt;
                    }

                    Regex regexNm = new Regex(@"^[\d|\+][\d]*$");
                    if (!regexNm.IsMatch(deliveryInfo.Mobile))
                    {
                        sysRslt.Message = "[" + Resources.Label.AddressMobileNum + "] " + Resources.Message.PleaseEnterCorrectPhoneNumber;
                        return sysRslt;
                    }
                }

                //郵編長度
                if (!string.IsNullOrEmpty(deliveryInfo.PostalCode))
                {
                    if (deliveryInfo.PostalCode.Length > 9)
                    {
                        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.AddressMobileNum, "9");
                        return sysRslt;
                    }
                }

                //國家地區選擇 & 郵編格式
                if (deliveryInfo.CountryId <= 0)
                {
                    //sysRslt.Message = "[" + Resources.Label.AddressDestination + "] " + Resources.Message.FieldRequire;
                    sysRslt.Message = Resources.Message.ZipInconCountry;
                    return sysRslt;
                }
                else
                {
                    //國家/地區不為空，則需檢查 省份/區域，和郵編
                    int countryId = deliveryInfo.CountryId;
                    int provinceId = deliveryInfo.ProvinceId;
                    string postalCode = deliveryInfo.PostalCode;

                    //省份列表不為空時，必須指定省份
                    var provinceList = GetProvinces(countryId);
                    if (provinceList?.Count > 0 && provinceId <= 0)
                    {
                        //sysRslt.Message = "[" + Resources.Label.AddressDistrict + "] " + Resources.Message.FieldRequire;
                        sysRslt.Message = Resources.Message.EnterZipCode;
                        return sysRslt;
                    }

                    //檢查郵編是否必填
                    var country = GetCountry(countryId);
                    if (country != null)
                    {
                        if (country.IsNeedPostalCode && string.IsNullOrEmpty(postalCode))
                        {
                            //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.FieldRequire;
                            sysRslt.Message = Resources.Message.ZipInconCountry;
                            return sysRslt;
                        }
                    }

                    #region 特殊地區的郵編正則檢查

                    string zipCodeRegex = string.Empty;
                    if (countryId == (int)CountryType.Australia_WA || countryId == (int)CountryType.Australia_Other || countryId == (int)CountryType.Norway)
                    {
                        zipCodeRegex = @"^\d{4}$";
                        Regex regex = new Regex(zipCodeRegex);
                        if (!regex.IsMatch(postalCode))
                        {
                            //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.ZipInconCountry;
                            sysRslt.Message = Resources.Message.ZipInconCountry;
                            return sysRslt;
                        }
                    }
                    else if (countryId == (int)CountryType.Brazil)
                    {
                        zipCodeRegex = @"^\d{5}-\d{3}$";
                        Regex regex = new Regex(zipCodeRegex);
                        if (!regex.IsMatch(postalCode))
                        {
                            //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.ZipInconCountry;
                            sysRslt.Message = Resources.Message.ZipInconCountry;
                            return sysRslt;
                        }
                    }
                    else if (countryId == (int)CountryType.Canada)
                    {
                        zipCodeRegex = @"^\w\d\w \d\w\d$";
                        Regex regex = new Regex(zipCodeRegex);
                        if (!regex.IsMatch(postalCode))
                        {
                            //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.ZipInconCountry;
                            sysRslt.Message = Resources.Message.ZipInconCountry;
                            return sysRslt;
                        }
                    }
                    else if (countryId == (int)CountryType.Ecuador || countryId == (int)CountryType.RussianFederation || countryId == (int)CountryType.Singapore || countryId == (int)CountryType.Vietnam)
                    {
                        zipCodeRegex = @"^\d{6}$";
                        Regex regex = new Regex(zipCodeRegex);
                        if (!regex.IsMatch(postalCode))
                        {
                            //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.ZipInconCountry;
                            sysRslt.Message = Resources.Message.ZipInconCountry;
                            return sysRslt;
                        }
                    }
                    else if (countryId == (int)CountryType.France || countryId == (int)CountryType.Germany || countryId == (int)CountryType.Senegal || countryId == (int)CountryType.USA_Hawaii || countryId == (int)CountryType.USA_NewYork || countryId == (int)CountryType.USA_OtherStates)
                    {
                        zipCodeRegex = @"^\d{5}$";
                        Regex regex = new Regex(zipCodeRegex);
                        if (!regex.IsMatch(postalCode))
                        {
                            //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.ZipInconCountry;
                            sysRslt.Message = Resources.Message.ZipInconCountry;
                            return sysRslt;
                        }
                    }
                    else if (countryId == (int)CountryType.SouthAfrica)
                    {
                        zipCodeRegex = @"^(\d{5})|(\d{6})$";
                        Regex regex = new Regex(zipCodeRegex);
                        if (!regex.IsMatch(postalCode))
                        {
                            //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.ZipInconCountry;
                            sysRslt.Message = Resources.Message.ZipInconCountry;
                            return sysRslt;
                        }
                    }
                    else if (countryId == (int)CountryType.UnitedKingdom)
                    {
                        zipCodeRegex = @"^(\w\d \d\w\w)|(\w\d\d \d\w\w)|(\w\d\w \d\w\w)|(\w\w\d \d\w\w)|(\w\w\d\d \d\w\w)|(\w\w\d\w \d\w\w)$";
                        Regex regex = new Regex(zipCodeRegex);
                        if (!regex.IsMatch(postalCode))
                        {
                            //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.ZipInconCountry;
                            sysRslt.Message = Resources.Message.ZipInconCountry;
                            return sysRslt;
                        }
                    }
                    else if (countryId == (int)CountryType.ChinaMainland)
                    {
                        if (provinceId == 10)//廣東
                        {
                            if (!string.IsNullOrEmpty(postalCode))
                            {
                                zipCodeRegex = @"^[5][1-2][0-9]\d{3}(?!\d)";
                                Regex regex = new Regex(zipCodeRegex);
                                if (!regex.IsMatch(postalCode))
                                {
                                    //sysRslt.Message = "[" + Resources.Label.AddressPostal + "] " + Resources.Message.ZipInconCountry;
                                    sysRslt.Message = Resources.Message.ZipInconProvince;
                                    return sysRslt;
                                }
                            }
                            else
                            {
                                sysRslt.Message = Resources.Message.EnterZipCode; //"[" + Resources.Label.AddressPostal + "] " + Resources.Message.FieldRequire;
                                return sysRslt;
                            }
                        }
                    }

                    #endregion
                }

                int maxAddrEnLen = 35;
                //int maxAddrCnLen = 22;
                //地址一
                if (string.IsNullOrEmpty(deliveryInfo.Address))
                {
                    sysRslt.Message = "[" + Resources.Label.Address + "] " + Resources.Message.FieldRequire;
                    return sysRslt;
                }
                else
                {
                    //if (lang == Language.E)
                    //{
                    //    if (deliveryInfo.Address.Length > maxAddrEnLen)
                    //    {
                    //        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.Address, maxAddrEnLen.ToString());
                    //        return sysRslt;
                    //    }
                    //}
                    //else
                    //{
                    //    if (deliveryInfo.Address.Length > maxAddrCnLen)
                    //    {
                    //        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.Address, maxAddrCnLen.ToString());
                    //        return sysRslt;
                    //    }
                    //}
                    if (!UTF8StringLengthChecking(deliveryInfo.Address, maxAddrEnLen))
                    {
                        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.Address, maxAddrEnLen.ToString());
                        return sysRslt;
                    }
                }

                //地址二
                if (!string.IsNullOrEmpty(deliveryInfo.Address1))
                {
                    if (!UTF8StringLengthChecking(deliveryInfo.Address1, maxAddrEnLen))
                    {
                        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.Address, maxAddrEnLen.ToString());
                        return sysRslt;
                    }
                }

                //地址三
                if (!string.IsNullOrEmpty(deliveryInfo.Address2))
                {
                    if (!UTF8StringLengthChecking(deliveryInfo.Address2, maxAddrEnLen))
                    {
                        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.Address, maxAddrEnLen.ToString());
                        return sysRslt;
                    }
                }

                //地址四
                if (!string.IsNullOrEmpty(deliveryInfo.Address3))
                {
                    if (!UTF8StringLengthChecking(deliveryInfo.Address3, maxAddrEnLen))
                    {
                        sysRslt.Message = string.Format(lengthOverFormat, Resources.Label.Address, maxAddrEnLen.ToString());
                        return sysRslt;
                    }
                }

                sysRslt.Succeeded = true;
            }

            return sysRslt;
        }

        /// <summary>
        /// 使用UTF8編碼檢查指定內容的字節長度
        /// </summary>
        /// <param name="content">檢查內容</param>
        /// <param name="maxEnLen">最大長度</param>
        /// <returns></returns>
        private bool UTF8StringLengthChecking(string content, int maxEnLen)
        {
            bool res = true;
            //int maxEnLen = 35;
            int length = Encoding.UTF8.GetByteCount(content);
            if (length > maxEnLen)
            {
                res = false;
            }
            return res;
        }
    }
}
