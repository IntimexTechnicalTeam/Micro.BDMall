using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class MerchantBLL : BaseBLL, IMerchantBLL
    {
        IMerchantRepository merchantRepository;
        ITranslationRepository translationRepository;
        IMerchantShipMethodMappingRepository merchantShipMethodMappingRepository;
        IUserBLL userBLL;

        PreHeatMerchantService mchHeatService;

        public MerchantBLL(IServiceProvider services) : base(services)
        {
            merchantRepository = Services.Resolve<IMerchantRepository>();
            translationRepository = Services.Resolve<ITranslationRepository>();
            merchantShipMethodMappingRepository = Services.Resolve<IMerchantShipMethodMappingRepository>();
            userBLL = Services.Resolve<IUserBLL>();

            mchHeatService = (PreHeatMerchantService)Services.GetService(typeof(PreHeatMerchantService));
        }

        public List<KeyValue> GetMerchantCboSrcByCond(bool containMall)
        {
            var merchantList = new List<KeyValue>();

            var isMerchant = CurrentUser.LoginType <= LoginType.ThirdMerchantLink ? true : false;
            if (containMall && !isMerchant)
                merchantList.Add(new KeyValue { Id = Guid.Empty.ToString(), Text = BDMall.Resources.Value.Mall });

            var query = (from d in baseRepository.GetList<Merchant>()
                         join t in baseRepository.GetList<Translation>() on new { a1 = d.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into TransTemp
                         from tt in TransTemp.DefaultIfEmpty()
                         where (!isMerchant || (isMerchant && d.Id == Guid.Parse(CurrentUser.UserId))) && d.IsDeleted == false
                         select new KeyValue
                         {
                             Id = d.Id.ToString().ToLower(),
                             Text = tt != null ? tt.Value : string.Empty,
                         });

            var list = query.OrderBy(o => o.Text).ToList();
            merchantList.AddRange(list);
            return merchantList;
        }

        public PageData<MerchantView> GetMerchLstByCond(MerchantPageInfo condition)
        {
            PageData<MerchantView> merchLst = new PageData<MerchantView>();

            merchLst = merchantRepository.SearchMerchByCond(condition);
            foreach (var item in merchLst.Data)
            {
                var promotion = baseRepository.GetList<MerchantPromotion>(x => x.MerchantId == item.Id && x.IsActive && x.IsDeleted).OrderByDescending(o => o.CreateDate).FirstOrDefault();
                if (promotion != null)
                    item.ApproveStatus = promotion.ApproveStatus;
                else
                    item.ApproveStatus = ApproveType.Editing;
                item.Score = baseRepository.GetModel<MerchantStatistic>(x => x.MerchId == item.Id && x.IsActive && !x.IsDeleted)?.Score ?? 0;
                item.IsAccountCreated = userBLL.CheckMerchantAccountExist(item.Id);
            }

            return merchLst;

        }

        public MerchantView GetMerchById(Guid Id)
        {
            var merchant = new MerchantView();

            if (Id == Guid.Empty)
            {
                merchant.NameList = translationRepository.GetMutiLanguage(Guid.Empty);
                merchant.ContactList = translationRepository.GetMutiLanguage(Guid.Empty);
                merchant.ContactAddrList = translationRepository.GetMutiLanguage(Guid.Empty);
                merchant.ContactAddr2List = translationRepository.GetMutiLanguage(Guid.Empty);
                merchant.ContactAddr3List = translationRepository.GetMutiLanguage(Guid.Empty);
                merchant.ContactAddr4List = translationRepository.GetMutiLanguage(Guid.Empty);
                merchant.RemarksList = translationRepository.GetMutiLanguage(Guid.Empty);
                return merchant;
            }

            var dbMch = baseRepository.GetModelById<Merchant>(Id);
            if (dbMch == null)
                return merchant;

            if (dbMch.IsDeleted)
                return merchant;

            var dbEcShip = baseRepository.GetModelById<MerchantECShipInfo>(Id);

            merchant = AutoMapperExt.MapTo<MerchantView>(dbMch);
            merchant.Lang = merchant.Language;
            if (dbEcShip != null)
                merchant.ECShipInfo = AutoMapperExt.MapTo<MerchantECShipInfoDto>(dbEcShip);

            //名稱
            merchant.NameList = translationRepository.GetMutiLanguage(merchant.NameTransId);
            merchant.Name = translationRepository.GetDescForLang(merchant.NameTransId, CurrentUser.Lang);
            //聯繫人
            merchant.ContactList = translationRepository.GetMutiLanguage(merchant.ContactTransId);
            merchant.Contact = translationRepository.GetDescForLang(merchant.ContactTransId, CurrentUser.Lang);
            //地址
            merchant.ContactAddrList = translationRepository.GetMutiLanguage(merchant.ContactAddrTransId);
            merchant.ContactAddress = translationRepository.GetDescForLang(merchant.ContactAddrTransId, CurrentUser.Lang);
            //地址2
            merchant.ContactAddr2List = translationRepository.GetMutiLanguage(merchant.ContactAddr2TransId);
            merchant.ContactAddress2 = translationRepository.GetDescForLang(merchant.ContactAddr2TransId, CurrentUser.Lang);
            //地址3
            merchant.ContactAddr3List = translationRepository.GetMutiLanguage(merchant.ContactAddr3TransId);
            merchant.ContactAddress3 = translationRepository.GetDescForLang(merchant.ContactAddr3TransId, CurrentUser.Lang);
            //地址4
            merchant.ContactAddr4List = translationRepository.GetMutiLanguage(merchant.ContactAddr4TransId);
            merchant.ContactAddress4 = translationRepository.GetDescForLang(merchant.ContactAddr4TransId, CurrentUser.Lang);
            //備註
            merchant.RemarksList = translationRepository.GetMutiLanguage(merchant.RemarksTransId);
            merchant.Remarks = translationRepository.GetDescForLang(merchant.RemarksTransId, CurrentUser.Lang);

            merchant.CustomUrls = baseRepository.GetList<CustomUrl>(x => x.KeyType == CustomUrlType.Merchant && x.KeyId == merchant.Id.ToString()).Select(s => s.Url).ToList();

            return merchant;
        }

        public async Task<SystemResult> Save(MerchantView merchVw)
        {
            var result = new SystemResult();

            FillContactAddressDefault(merchVw);
            if (merchVw.CustomUrls != null && merchVw.CustomUrls.Any())
            {
                var flag = baseRepository.Any<CustomUrl>(p => merchVw.CustomUrls.Contains(p.Url) && p.KeyId != merchVw.Id.ToString());
                if (flag)
                    throw new BLException(Resources.Message.CustomUrlIsExist);
            }

            if (merchVw.RecStatus == RecordStatus.Add)
            {
                result = CreateMerchant(merchVw);
            }
            else
            {
                result = UpdateMerchant(merchVw);
            }

            await mchHeatService.CreatePreHeat(merchVw.Id);
            return result;
        }

        public async Task<SystemResult> ActiveMerchantAsync(Guid merchID)
        {
            var result = new SystemResult();
            var merchant = baseRepository.GetModelById<Merchant>(merchID);
            if (merchant == null)
                throw new BLException("不存在的商家");

            merchant.IsActive = true;
            merchant.UpdateDate = DateTime.Now;
            baseRepository.Update(merchant);

            await UpdateCache(merchant.Id, MchAction.Active);
            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> DeactiveMerchantAsync(Guid merchID)
        {
            var result = new SystemResult();
            var merchant = baseRepository.GetModelById<Merchant>(merchID);
            if (merchant == null)
                throw new BLException("不存在的商家");

            merchant.IsActive = false;
            merchant.UpdateDate = DateTime.Now;

            var prodList = baseRepository.GetList<Product>(d => d.MerchantId == merchID && d.Status == ProductStatus.OnSale || d.Status == ProductStatus.Pass).ToList();
            foreach (var item in prodList)
            {
                item.Status = ProductStatus.AutoOffSale;
                item.UpdateDate = DateTime.Now;
            }

            using var tran = baseRepository.CreateTransation();
            baseRepository.Update(merchant);
            baseRepository.Update(prodList);
            tran.Commit();

            await UpdateCache(merchID, MchAction.DeActive);
            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> LogicalDelMerchRec(string recIDsList)
        {
            var result = new SystemResult();

            if (recIDsList.IsEmpty())
                throw new BLException(MerchantErrorEnm.IncomingDataEmpty.ToString());

            var list = recIDsList.Split(',').ToList();
            var dbMchList = baseRepository.GetList<Merchant>(x => list.Contains(x.Id.ToString()));
            foreach (var item in dbMchList)
            {
                item.IsDeleted = true;
                item.UpdateDate = DateTime.Now;
            }
            baseRepository.Update(dbMchList);

            foreach (var item in dbMchList)
            {
                await UpdateCache(item.Id, MchAction.DeActive);
            }
            result.Succeeded = true;
            return result;
        }

        public MerchantShipMethodMappingView GetMerchantShipMethods(Guid merchantId)
        {
            var view = new MerchantShipMethodMappingView();
            var defaultShipMethods = merchantShipMethodMappingRepository.GetShipMethidByMerchantId(Guid.Empty).Where(p => p.IsEffect).OrderBy(o => o.ShipCode).ToList();
            var dbShipmethods = merchantShipMethodMappingRepository.GetShipMethidByMerchantId(merchantId).OrderBy(o => o.ShipCode).ToList();

            if (dbShipmethods == null || !dbShipmethods.Any())
            {
                view.MerchantId = merchantId;

                view.MerchantShipMethods = defaultShipMethods.Select(s => new MerchantShipMethodView {
                    ShipMethodCode = s.ShipCode,
                    IsEffect = false,
                    ShipMethodName = s.ShipMethodName,
                }).ToList();

                if (CurrentUser.LoginType <= LoginType.ThirdMerchantLink)
                    view.MerchantShipMethods = view.MerchantShipMethods.Where(p => p.IsEffect).ToList();

                return view;
            }

            return GenMerchantShipMethodView(defaultShipMethods, dbShipmethods);
        }

        public void SaveShipMethodMapping(MerchantShipMethodMappingView mappingShipMethod)
        {
            if (mappingShipMethod.MerchantShipMethods != null)
            {
                var isMerchant = mappingShipMethod.MerchantId == Guid.Empty;

                var dbShipmethodsList = merchantShipMethodMappingRepository.GetShipMethidByMerchantId(mappingShipMethod.MerchantId);
                var dbShipmethods = AutoMapperExt.MapTo<List<MerchantActiveShipMethod>>(dbShipmethodsList);

                var newShipMethods = new List<MerchantActiveShipMethod>();
                var oldShipMethods = new List<MerchantActiveShipMethod>();
                var disActiveShipMethods = new List<MerchantActiveShipMethod>();
                foreach (var item in mappingShipMethod.MerchantShipMethods)
                {
                    var shipMethod = dbShipmethods.FirstOrDefault(p => p.ShipCode == item.ShipMethodCode);
                    if (shipMethod != null)
                    {                        
                        if (isMerchant && item.IsEffect == false)//当STPAdmin取消一种付运方式时，其它商家的该种付运方式都设为失效
                        {                           
                            var otherShipMethods = baseRepository.GetList<MerchantActiveShipMethod>(x=>x.ShipCode == item.ShipMethodCode && x.IsEffect).ToList();
                            foreach (var a in otherShipMethods)  a.IsEffect = false;                          
                            disActiveShipMethods.AddRange(otherShipMethods);
                        }
                        shipMethod.IsEffect = item.IsEffect;
                        shipMethod.UpdateBy = Guid.Parse(CurrentUser.UserId);
                        shipMethod.UpdateDate = DateTime.Now;
                        oldShipMethods.Add(shipMethod);
                    }
                    else
                    {
                        MerchantActiveShipMethod a = new MerchantActiveShipMethod();
                        a.Id = Guid.NewGuid();                   
                        a.MerchantId = mappingShipMethod.MerchantId;
                        a.ShipCode = item.ShipMethodCode;
                        a.IsEffect = item.IsEffect;
                        newShipMethods.Add(a);
                    }
                }

                using var tran = baseRepository.CreateTransation();
                baseRepository.Update(oldShipMethods);
                baseRepository.Insert(newShipMethods);
                baseRepository.Update(disActiveShipMethods);
                tran.Commit();
            }
        }

        private MerchantShipMethodMappingView GenMerchantShipMethodView(List<MerchantActiveShipMethodDto> defaultShipMethod, List<MerchantActiveShipMethodDto> shipMethods)
        {
            MerchantShipMethodMappingView view = new MerchantShipMethodMappingView();
            if (shipMethods !=null && shipMethods.Any())
            {               
                view.MerchantShipMethods = defaultShipMethod.Select(s =>
                                        new MerchantShipMethodView
                                        {
                                            IsEffect = shipMethods.FirstOrDefault(p => p.ShipCode == s.ShipCode)?.IsEffect ?? false,
                                            ShipMethodCode = s.ShipCode,
                                            ShipMethodName = s.ShipMethodName,
                                        }).ToList();

                if (CurrentUser.LoginType <= LoginType.ThirdMerchantLink)            
                    view.MerchantShipMethods = view.MerchantShipMethods.Where(p => p.IsEffect).ToList();
                              
                view.MerchantId = shipMethods[0].MerchantId;
            }
            return view;
        }

        private SystemResult CreateMerchant(MerchantView merchVw)
        {
            var result = new SystemResult();
            var recInsert = AutoMapperExt.MapTo<Merchant>(merchVw);

            recInsert.MerchNo = AutoGenerateNumber();
            recInsert.IsActive = false;
            recInsert.UpdateDate = DateTime.Now;    
            if (recInsert.IsExternal)
            {
                recInsert.AppId = "";
                recInsert.AppSecret = "";
            }
            else
            {
                recInsert.AppId = recInsert.AppId ?? "";
                recInsert.AppSecret = recInsert.AppSecret ?? "";
            }

            #region 处理Merchant主表

            recInsert.Id = Guid.NewGuid();
            recInsert.NameTransId = Guid.NewGuid();
            recInsert.ContactTransId = Guid.NewGuid();
            recInsert.ContactAddrTransId = Guid.NewGuid();
            recInsert.ContactAddr2TransId = Guid.NewGuid();
            recInsert.ContactAddr3TransId = Guid.NewGuid();
            recInsert.ContactAddr4TransId = Guid.NewGuid();
            recInsert.RemarksTransId = Guid.NewGuid();

            var NameList = translationRepository.GenTranslations(merchVw.NameList, TranslationType.Merchant, recInsert.NameTransId);
            var ContactTransList = translationRepository.GenTranslations(merchVw.ContactList, TranslationType.Merchant, recInsert.ContactTransId);
            var ContactAddrList = translationRepository.GenTranslations(merchVw.ContactAddrList, TranslationType.Merchant, recInsert.ContactAddrTransId);
            var ContactAddr2List = translationRepository.GenTranslations(merchVw.ContactAddr2List, TranslationType.Merchant, recInsert.ContactAddr2TransId);
            var ContactAddr3List = translationRepository.GenTranslations(merchVw.ContactAddr3List, TranslationType.Merchant, recInsert.ContactAddr3TransId);
            var ContactAddr4List = translationRepository.GenTranslations(merchVw.ContactAddr4List, TranslationType.Merchant, recInsert.ContactAddr4TransId);
            var RemarksList = translationRepository.GenTranslations(merchVw.RemarksList, TranslationType.Merchant, recInsert.RemarksTransId);

            #endregion

            #region 处理商家自定义链接

            List<CustomUrl> newCustomUrls = new List<CustomUrl>();
            foreach (var item in merchVw.CustomUrls)
            {
                newCustomUrls.Add(new CustomUrl
                {
                    KeyId = recInsert.Id.ToString(),
                    KeyType = CustomUrlType.Merchant,
                    Url = item
                });
            }

            #endregion

            var ECShipInfo = new  MerchantECShipInfo
            {
                Id = recInsert.Id,
                //ClientId = UnitOfWork.Operator.ClientId,
                CreateBy = Guid.Parse(CurrentUser.UserId),
                CreateDate = DateTime.Now,
                ECShipEmail = merchVw.ECShipInfo.ECShipEmail,
                ECShipIntegraterName = merchVw.ECShipInfo.ECShipIntegraterName,
                ECShipName = merchVw.ECShipInfo.ECShipName,
                ECShipPassword = merchVw.ECShipInfo.ECShipPassword,
                SPIntegraterName = merchVw.ECShipInfo.SPIntegraterName,
                SPName = merchVw.ECShipInfo.SPName,
                SPPassword = merchVw.ECShipInfo.SPPassword,
            };
            
            var shipMethodCMLst = merchantShipMethodMappingRepository.GetShipMethidByMerchantId(Guid.Empty).Where(p => p.IsEffect == true).ToList();
            var shipMethordLst = AutoMapperExt.MapTo<List<MerchantActiveShipMethod>>(shipMethodCMLst);
            foreach (var item in shipMethordLst)
            {
                item.Id = Guid.NewGuid();
                item.MerchantId = recInsert.Id;                                           
            }

            using var tran = baseRepository.CreateTransation();
            baseRepository.Insert(recInsert);
            baseRepository.Insert(NameList);
            baseRepository.Insert(ContactTransList);
            baseRepository.Insert(ContactAddrList);
            baseRepository.Insert(ContactAddr2List);
            baseRepository.Insert(ContactAddr3List);
            baseRepository.Insert(ContactAddr4List);
            baseRepository.Insert(RemarksList);

            baseRepository.Insert(newCustomUrls);
            baseRepository.Insert(ECShipInfo);
            baseRepository.Insert(shipMethordLst);

            tran.Commit();
            result.Succeeded = true;

            return result;
        }

        private SystemResult UpdateMerchant(MerchantView recUpdate)
        {
            var result = new SystemResult();

            var merchOld = baseRepository.GetModelById<Merchant>(recUpdate.Id);
            if (merchOld == null)
                throw new BLException(MerchantErrorEnm.RecordNotExsit.ToString());

            var dbECShipInfo = baseRepository.GetModelById<MerchantECShipInfo>(recUpdate.Id);

            #region 处理主表

            merchOld.FaxNum = recUpdate.FaxNum;          
            merchOld.ContactEmail = recUpdate.ContactEmail;
            merchOld.OrderEmail = recUpdate.OrderEmail;

            merchOld.IsActive = recUpdate.IsActive;         
            merchOld.ContactPhoneNum = recUpdate.ContactPhoneNum;
            merchOld.GCP = recUpdate.GCP;           
            merchOld.IsExternal = recUpdate.IsExternal;
            merchOld.Language = recUpdate.Lang;           
            merchOld.CommissionRate = recUpdate.CommissionRate;
            merchOld.IsTransin = recUpdate.IsTransin;
            merchOld.IsHongKong = recUpdate.IsHongKong;
            merchOld.BankAccount = recUpdate.BankAccount;
            merchOld.UpdateBy = Guid.Parse(CurrentUser.UserId);
            merchOld.UpdateDate = DateTime.Now;

            if (merchOld.IsExternal)
            {
                merchOld.AppId = "";
                merchOld.AppSecret = "";
            }
            else
            {
                merchOld.AppId = recUpdate.AppId ?? "";
                merchOld.AppSecret = recUpdate.AppSecret ?? "";
            }

            dbECShipInfo.SPName = recUpdate.ECShipInfo.SPName;
            dbECShipInfo.SPIntegraterName = recUpdate.ECShipInfo.SPIntegraterName;
            dbECShipInfo.SPPassword = recUpdate.ECShipInfo.SPPassword;
            dbECShipInfo.ECShipEmail = recUpdate.ECShipInfo.ECShipEmail;
            dbECShipInfo.ECShipName = recUpdate.ECShipInfo.ECShipName;
            dbECShipInfo.ECShipIntegraterName = recUpdate.ECShipInfo.ECShipIntegraterName;
            dbECShipInfo.ECShipPassword = recUpdate.ECShipInfo.ECShipPassword;
            dbECShipInfo.UpdateDate = DateTime.Now;
            dbECShipInfo.UpdateBy = Guid.Parse(CurrentUser.UserId);

            #endregion

            var NameList = translationRepository.GenTranslations(recUpdate.NameList, TranslationType.Merchant, recUpdate.NameTransId,ActionTypeEnum.Modify);
            var ContactTransList = translationRepository.GenTranslations(recUpdate.ContactList, TranslationType.Merchant, recUpdate.ContactTransId, ActionTypeEnum.Modify);
            var ContactAddrList = translationRepository.GenTranslations(recUpdate.ContactAddrList, TranslationType.Merchant, recUpdate.ContactAddrTransId, ActionTypeEnum.Modify);
            var ContactAddr2List = translationRepository.GenTranslations(recUpdate.ContactAddr2List, TranslationType.Merchant, recUpdate.ContactAddr2TransId, ActionTypeEnum.Modify);
            var ContactAddr3List = translationRepository.GenTranslations(recUpdate.ContactAddr3List, TranslationType.Merchant, recUpdate.ContactAddr3TransId, ActionTypeEnum.Modify);
            var ContactAddr4List = translationRepository.GenTranslations(recUpdate.ContactAddr4List, TranslationType.Merchant, recUpdate.ContactAddr4TransId, ActionTypeEnum.Modify);
            var RemarksList = translationRepository.GenTranslations(recUpdate.RemarksList, TranslationType.Merchant, recUpdate.RemarksTransId, ActionTypeEnum.Modify);

            #region 处理商家自定义链接

            var deleteCustomUrls = baseRepository.GetList<CustomUrl>(p => p.KeyType == CustomUrlType.Merchant && p.KeyId == recUpdate.Id.ToString()).ToList();
            List<CustomUrl> newCustomUrls = new List<CustomUrl>();
            foreach (var item in recUpdate.CustomUrls)
            {
                newCustomUrls.Add(new CustomUrl
                {
                    KeyId = recUpdate.Id.ToString(),
                    KeyType = CustomUrlType.Merchant,
                    Url = item
                });
            }

            #endregion

            var deleteRole = new List<UserRole>();
            var userRole = new List<UserRole>();
            var mchUserRole = GenMerchantUserRole(recUpdate);
            if (mchUserRole != null)
            {
                deleteRole = mchUserRole.Item1;
                userRole = mchUserRole.Item2;
            }

            using var tran = baseRepository.CreateTransation();
            baseRepository.Update(merchOld);
            baseRepository.Update(dbECShipInfo);

            baseRepository.Delete(deleteCustomUrls);
            baseRepository.Insert(newCustomUrls);

            baseRepository.Delete(deleteRole);
            baseRepository.Insert(userRole);

            baseRepository.Update(NameList);
            baseRepository.Update(ContactTransList);
            baseRepository.Update(ContactAddrList);
            baseRepository.Update(ContactAddr2List);
            baseRepository.Update(ContactAddr3List);
            baseRepository.Update(ContactAddr4List);
            baseRepository.Update(RemarksList);
            tran.Commit();

            result.Succeeded = true;
            return result;
        }

        Tuple<List<UserRole>, List<UserRole>> GenMerchantUserRole(MerchantView merchant)
        {
            var user = baseRepository.GetModel<User>(x => x.MerchantId == merchant.Id);
            if (user == null)
                return null;

            var userRoles = baseRepository.GetList<UserRole>(x => x.UserId == user.Id);
            var extRoleId = Guid.Parse(StoreConst.ExternalMerchantAdminRoleId);
            var extGS1RoleId = Guid.Parse(StoreConst.ExternalGS1MerchantAdminRoleId);
            var intRoleId = Guid.Parse(StoreConst.InternalMerchantAdminRoleId);
            var intGS1RoleId = Guid.Parse(StoreConst.InternalGS1MerchantAdminRoleId);

            UserRole userRole = new UserRole();
            if (merchant.IsExternal)
            {
                if (merchant.MerchantType == MerchantType.GS1)
                {
                    userRole.RoleId = extGS1RoleId;
                }
                else
                {
                    userRole.RoleId = extRoleId;
                }
            }
            else
            {
                if (merchant.MerchantType == MerchantType.GS1)
                {
                    userRole.RoleId = intGS1RoleId;
                }
                else
                {
                    userRole.RoleId = intRoleId;
                }
            }

            userRole.Id = Guid.NewGuid();
            userRole.UserId = user.Id;
            var insertRole = new  List<UserRole>();
            insertRole.Add(userRole); 
            var deleteRole = new List<UserRole>();
            foreach (var item in userRoles)
            {
                if (item.RoleId == extRoleId || item.RoleId == extGS1RoleId || item.RoleId == intRoleId || item.RoleId == intGS1RoleId)
                    deleteRole.Add(item);
            }

            var result = new Tuple<List<UserRole>, List<UserRole>> (deleteRole, insertRole);
            return result;
        }

        private void FillContactAddressDefault(MerchantView merchVw)
        {
            var eContactValue = merchVw.ContactList.FirstOrDefault(p => p.Language == Language.E)?.Desc;
            if (eContactValue != "")
            {
                var cValue = merchVw.ContactList.FirstOrDefault(p => p.Language == Language.C)?.Desc ?? "";
                var sValue = merchVw.ContactList.FirstOrDefault(p => p.Language == Language.S)?.Desc ?? "";
                if (cValue == "")
                {
                    merchVw.ContactList.FirstOrDefault(p => p.Language == Language.C).Desc = eContactValue;
                }
                if (sValue == "")
                {
                    merchVw.ContactList.FirstOrDefault(p => p.Language == Language.S).Desc = eContactValue;
                }
            }

            var eAddressValue = merchVw.ContactAddrList.FirstOrDefault(p => p.Language == Language.E)?.Desc;
            if (merchVw.ContactAddrList.FirstOrDefault(p => p.Language == Language.E)?.Desc != "")
            {
                var cValue = merchVw.ContactAddrList.FirstOrDefault(p => p.Language == Language.C)?.Desc ?? "";
                var sValue = merchVw.ContactAddrList.FirstOrDefault(p => p.Language == Language.S)?.Desc ?? "";
                if (cValue == "")
                {
                    merchVw.ContactAddrList.FirstOrDefault(p => p.Language == Language.C).Desc = eAddressValue;
                }
                if (sValue == "")
                {
                    merchVw.ContactAddrList.FirstOrDefault(p => p.Language == Language.S).Desc = eAddressValue;
                }

            }

            var eAddressValue2 = merchVw.ContactAddr2List.FirstOrDefault(p => p.Language == Language.E)?.Desc;
            if (merchVw.ContactAddr2List.FirstOrDefault(p => p.Language == Language.E)?.Desc != "")
            {
                var cValue = merchVw.ContactAddr2List.FirstOrDefault(p => p.Language == Language.C)?.Desc ?? "";
                var sValue = merchVw.ContactAddr2List.FirstOrDefault(p => p.Language == Language.S)?.Desc ?? "";
                if (cValue == "")
                {
                    merchVw.ContactAddr2List.FirstOrDefault(p => p.Language == Language.C).Desc = eAddressValue2;
                }
                if (sValue == "")
                {
                    merchVw.ContactAddr2List.FirstOrDefault(p => p.Language == Language.S).Desc = eAddressValue2;
                }
            }

            var eAddressValue3 = merchVw.ContactAddr3List.FirstOrDefault(p => p.Language == Language.E)?.Desc;
            if (merchVw.ContactAddr3List.FirstOrDefault(p => p.Language == Language.E)?.Desc != "")
            {
                var cValue = merchVw.ContactAddr3List.FirstOrDefault(p => p.Language == Language.C)?.Desc ?? "";
                var sValue = merchVw.ContactAddr3List.FirstOrDefault(p => p.Language == Language.S)?.Desc ?? "";
                if (cValue == "")
                {
                    merchVw.ContactAddr3List.FirstOrDefault(p => p.Language == Language.C).Desc = eAddressValue3;
                }
                if (sValue == "")
                {
                    merchVw.ContactAddr3List.FirstOrDefault(p => p.Language == Language.S).Desc = eAddressValue3;
                }
            }

            var eAddressValue4 = merchVw.ContactAddr4List.FirstOrDefault(p => p.Language == Language.E)?.Desc;
            if (merchVw.ContactAddr4List.FirstOrDefault(p => p.Language == Language.E)?.Desc != "")
            {
                var cValue = merchVw.ContactAddr4List.FirstOrDefault(p => p.Language == Language.C)?.Desc ?? "";
                var sValue = merchVw.ContactAddr4List.FirstOrDefault(p => p.Language == Language.S)?.Desc ?? "";
                if (cValue == "")
                {
                    merchVw.ContactAddr4List.FirstOrDefault(p => p.Language == Language.C).Desc = eAddressValue4;
                }
                if (sValue == "")
                {
                    merchVw.ContactAddr4List.FirstOrDefault(p => p.Language == Language.S).Desc = eAddressValue4;
                }
            }
        }

        private async Task UpdateCache(Guid MchId, MchAction action = MchAction.Apporve)
        {
            string key = string.Empty;
            if (action <= MchAction.Active)
            {
                //更新商家缓存
                await mchHeatService.CreatePreHeat(MchId);
                var flag = baseRepository.Any<MerchantPromotion>(x => x.MerchantId == MchId && x.ApproveStatus == ApproveType.Pass);
                if (flag)
                {
                    //更新PromotionMerchant缓存
                    key = $"{PreHotType.Hot_PromotionMerchant}";
                    await mchHeatService.UpdatePromotionMerchant(key, MchId);

                    ////更新商家对应的商品PromotionProduct缓存
                    //key = $"{PreHotType.Hot_PromotionProduct}";
                    await mchHeatService.UpdatePromotionProductByMchId(key, MchId);
                }
            }
            else
            {
                //删除PromotionMerchant缓存
                key = $"{PreHotType.Hot_PromotionMerchant}";
                await mchHeatService.DeletePromotionMerchant(key, MchId);

                ////删除商家对应的商品PromotionProduct缓存
                await mchHeatService.DeletePromotionProductByMchId(key, MchId);

                //删除商家缓存
                key = $"{PreHotType.Hot_Merchants}_C";
                await mchHeatService.DeletePreHeat(key, MchId.ToString());

                key = $"{PreHotType.Hot_Merchants}_E";
                await mchHeatService.DeletePreHeat(key, MchId.ToString());

                key = $"{PreHotType.Hot_Merchants}_S";
                await mchHeatService.DeletePreHeat(key, MchId.ToString());
            }
        }

    }
}

