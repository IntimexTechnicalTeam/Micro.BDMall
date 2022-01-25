using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class MerchantBLL : BaseBLL, IMerchantBLL
    {
        IMerchantRepository merchantRepository;
        IMerchantPromotionRepository merchantPromotionRepository;
        ITranslationRepository translationRepository;
        IMerchantShipMethodMappingRepository merchantShipMethodMappingRepository;
        ICodeMasterRepository codeMasterRepository;
        IExpressCompanyRepository expressCompanyRepository;

        IUserBLL userBLL;
        ISettingBLL settingBLL;
        ICurrencyBLL currencyBLL;

        PreHeatMerchantService mchHeatService;

        public MerchantBLL(IServiceProvider services) : base(services)
        {
            merchantRepository = Services.Resolve<IMerchantRepository>();
            translationRepository = Services.Resolve<ITranslationRepository>();
            merchantShipMethodMappingRepository = Services.Resolve<IMerchantShipMethodMappingRepository>();
            codeMasterRepository = Services.Resolve<ICodeMasterRepository>();
            expressCompanyRepository = Services.Resolve<IExpressCompanyRepository>();
            userBLL = Services.Resolve<IUserBLL>();
            currencyBLL = Services.Resolve<ICurrencyBLL>();
            settingBLL = Services.Resolve<ISettingBLL>();
            merchantPromotionRepository = Services.Resolve<IMerchantPromotionRepository>();
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
                         where (!isMerchant || (isMerchant && d.Id == CurrentUser.MerchantId)) && d.IsDeleted == false
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
                var promotion = baseRepository.GetList<MerchantPromotion>(x => x.MerchantId == item.Id && x.IsActive && !x.IsDeleted).OrderByDescending(o => o.CreateDate).FirstOrDefault();
                if (promotion != null)
                    item.ApproveStatus = promotion.ApproveStatus;
                else
                    item.ApproveStatus = ApproveType.Editing;
                item.Score = baseRepository.GetModel<MerchantStatistic>(x => x.MerchId == item.Id && x.IsActive && !x.IsDeleted)?.Score ?? 0;
                item.IsAccountCreated = userBLL.CheckMerchantAccountExist(item.Id);
            }

            return merchLst;

        }

        public PageData<MerchantSelectSummary> SearchMerchantList(MerchantCond cond)
        {
            var data = new PageData<MerchantSelectSummary>();

            var query = (from m in  baseRepository.GetList<Merchant>()
                         join p in baseRepository.GetList<MerchantPromotion>() on new { a1 = m.Id, a2 = true, a3 = false } equals new { a1 = p.MerchantId, a2 = p.IsActive, a3 = p.IsDeleted }
                         join t in baseRepository.GetList<Translation>() on new { a1 = p.BigLogoId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         join n in baseRepository.GetList<Translation>() on new { a1 = m.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = n.TransId, a2 = n.Lang } into nc
                         from nn in nc.DefaultIfEmpty()
                         where (p.ApproveStatus == ApproveType.Pass || cond.ShowPass == false) && !m.IsDeleted
                         orderby m.UpdateDate descending
                         select new MerchantSelectSummary
                         {
                             Id = m.Id,
                             Logo = tt.Value,
                             Name = nn.Value,
                             IsActive = m.IsActive
                         });
            if (!string.IsNullOrEmpty(cond.Name))
                query = query.Where(d => d.Name.ToUpper().Contains(cond.Name));
            if (cond.IsActiveCond != null)
            {
                query = query.Where(d => d.IsActive == cond.IsActiveCond);
            }
            data.TotalRecord = query.Select(d => d.Id).Count();
            data.Data = query.Skip(cond.Offset).Take(cond.PageSize).ToList();
            return data;
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
        public MerchantShipMethodMappingView GetAdminShipMethod()
        {

            MerchantShipMethodMappingView view = new MerchantShipMethodMappingView();
            view.MerchantId = Guid.Empty;
            view.MerchantShipMethods = new List<MerchantShipMethodView>();

            var activeShipMethods = merchantShipMethodMappingRepository.GetShipMethidByMerchantId(Guid.Empty);
            var defaultShipMethods = codeMasterRepository.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.ShippingMethod).OrderBy(o => o.Key).ToList();
            var selfDefineShipMethods = expressCompanyRepository.GetActiveExpress();

            foreach (var item in defaultShipMethods)
            {
                var method = activeShipMethods.FirstOrDefault(p => p.ShipCode == item.Key.Trim());
                view.MerchantShipMethods.Add(new MerchantShipMethodView
                {
                    ShipMethodCode = item.Key,
                    ShipMethodName = item.Description,
                    IsEffect = method == null ? false : method.IsEffect

                });
            }
            foreach (var item in selfDefineShipMethods)
            {
                var method = activeShipMethods.FirstOrDefault(p => p.ShipCode == item.Code.Trim());
                view.MerchantShipMethods.Add(new MerchantShipMethodView
                {
                    ShipMethodCode = item.Code,
                    ShipMethodName = item.Name ?? "",
                    IsEffect = method == null ? false : method.IsEffect

                });
            }

            return view;

        }

        public MerchantShipMethodMappingView GetMerchantShipMethods(Guid merchantId)
        {
            var view = new MerchantShipMethodMappingView();
            var defaultShipMethods = merchantShipMethodMappingRepository.GetShipMethidByMerchantId(Guid.Empty).Where(p => p.IsEffect).OrderBy(o => o.ShipCode).ToList();
            var dbShipmethods = merchantShipMethodMappingRepository.GetShipMethidByMerchantId(merchantId).OrderBy(o => o.ShipCode).ToList();

            if (dbShipmethods == null || !dbShipmethods.Any())
            {
                view.MerchantId = merchantId;

                view.MerchantShipMethods = defaultShipMethods.Select(s => new MerchantShipMethodView
                {
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
                            var otherShipMethods = baseRepository.GetList<MerchantActiveShipMethod>(x => x.ShipCode == item.ShipMethodCode && x.IsEffect && x.MerchantId != Guid.Empty).ToList();
                            foreach (var a in otherShipMethods) a.IsEffect = false;
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

        public MerchantPromotionView GetMerchPromotionInfo(Guid merchID)
        {
            MerchantPromotionView view = new MerchantPromotionView();

            MerchantPromotion promotion = new MerchantPromotion();

            var editingPromotion = merchantPromotionRepository.GetNotApprovePromotion(merchID);

            if (editingPromotion != null)
            {
                promotion = editingPromotion;
            }
            else
            {
                promotion = merchantPromotionRepository.GetApprovePromotion(merchID);
            }

            if (promotion == null) return null;

            var fileServer = string.Empty;
            view.Id = promotion.Id;
            view.MerchantId = promotion.MerchantId;
            view.CoverId = promotion.CoverId; //fileServer + promotion.Cover + "?tid=" + (promotion.UpdateDate ?? DateTime.Now).ToString("ddmmss");
            view.LogoId = promotion.SmallLogoId; //fileServer + promotion.SmallLogo + "?tid=" + (promotion.UpdateDate ?? DateTime.Now).ToString("ddmmss");
            view.BigLogoId = promotion.BigLogoId;
            view.IntorductionTranId = promotion.IntorductionTranId;
            view.OrderTranId = promotion.OrderTransId;
            view.Covers = translationRepository.GetMutiLanguage(promotion.CoverId);
            view.Logos = translationRepository.GetMutiLanguage(promotion.SmallLogoId);
            view.BigLogos = translationRepository.GetMutiLanguage(promotion.BigLogoId);
            view.ExpCompleteDays = translationRepository.GetMutiLanguage(promotion.OrderTransId);

            view.ApproveStatus = promotion.ApproveStatus;
            view.Banners = baseRepository.GetList<MerchantPromotionBanner>(x => x.IsActive && !x.IsDeleted && x.PromotionId == view.Id)
                            .Select(b => new MerchantPromotionBannerView
                            {
                                Id = b.Id,
                                Image = b.Image,
                                IsDelete = false,
                                PromotionId = b.PromotionId,
                                Lang = b.Language,
                                Language = b.Language.ToString(),
                                Seq = b.Seq,
                                IsOpenWindow = b.IsOpenWindow,
                                BannerLink = b.BannerLink
                            }).OrderBy(o => o.Lang).ThenBy(t => t.Seq).ToList();


            view.ProductList = (from b in baseRepository.GetList<MerchantPromotionProduct>()
                                join c in baseRepository.GetList<Product>() on b.ProductCode equals c.Code
                                join s in baseRepository.GetList<ProductStatistics>() on b.ProductCode equals s.Code into cs
                                from ss in cs.DefaultIfEmpty()
                                where b.PromotionId == promotion.Id && !b.IsDeleted && b.IsActive && c.Status == ProductStatus.OnSale && !c.IsDeleted
                                select new MerchantPromotionProductView
                                {
                                    Id = b.Id,
                                    IsDelete = false,
                                    PromotionId = b.PromotionId,
                                    Code = b.ProductCode,
                                    ProductId = c.Id,
                                    NameTransId = ss == null ? Guid.NewGuid() : ss.InternalNameTransId,//c.NameTransId,
                                    Name = "",
                                    ListPrice = c.OriginalPrice,
                                    SalesPrice = c.SalePrice,
                                    CurrencyCode = c.CurrencyCode,
                                    MerchantId = c.MerchantId
                                }).ToList();

            foreach (var banner in view.Banners)
            {
                banner.Image = fileServer + banner.Image;
            }

            if (view.ProductList != null)
            {
                foreach (var item in view.ProductList)
                {
                    item.Imgs = GetProductImages(item.ProductId);
                    item.Name = translationRepository.GetTranslation(item.NameTransId, CurrentUser.Lang)?.Value ?? "";
                    item.Currency = currencyBLL.GetSimpleCurrency(item.CurrencyCode);
                }
            }
            var introductions = translationRepository.GetMutiLanguage(promotion.IntorductionTranId);
            var descs = translationRepository.GetMutiLanguage(promotion.DescTransId);
            var names = translationRepository.GetMutiLanguage(promotion.NameTranId);
            var tandcs = translationRepository.GetMutiLanguage(promotion.TAndCTranId);
            var notices = translationRepository.GetMutiLanguage(promotion.NoticeTranId);
            var rtnTermses = translationRepository.GetMutiLanguage(promotion.ReturnTermsTranId);

            view.PromotionNames = names;
            view.NameTranId = promotion.NameTranId;
            view.Descriptions = descs;
            view.DescTransId = promotion.DescTransId;
            view.PromotionIntroductions = introductions;
            view.TAndCTranId = promotion.TAndCTranId;
            view.TAndCs = tandcs;
            view.NoticeTranId = promotion.NoticeTranId;
            view.Notices = notices;
            view.ReturnTermsTranId = promotion.ReturnTermsTranId;
            view.ReturnTermses = rtnTermses;

            view.PromotionIntroduction = introductions.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.PromotionName = names.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.Description = descs.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.TAndC = tandcs.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.Notice = notices.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.ReturnTerms = rtnTermses.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.ExpCompleteDay = view.ExpCompleteDays.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc ?? "";
            view.LocalCoolDownDay = promotion.LocalCoolDownDay;
            view.OverSeaCoolDownDay = promotion.OverSeaCoolDownDay;

            view.ExpCompleteDay = string.IsNullOrEmpty(view.ExpCompleteDay) ? "2~4" : view.ExpCompleteDay;
            return view;
        }


        public List<string> GetProductImages(Guid prodID)
        {
            var productImages = new List<string>();

            var product = baseRepository.GetModelById<Product>(prodID);
            if (product != null)
            {
                var dbproductImage = baseRepository.GetModelById<ProductImage>(product.DefaultImage);
                if (dbproductImage != null)
                {
                    var imageItems = dbproductImage.ImageItems.OrderBy(o => o.Type).ToList();
                    if (imageItems != null)
                    {
                        var fileServer = string.Empty;
                        foreach (var item in imageItems)
                        {
                            productImages.Add(fileServer + item.Path);
                        }
                    }
                }
            }
            return productImages;
        }

        public bool SaveMerchantPromotion(MerchantPromotionView promotion)
        {
            promotion.Validate();

            var edittingPromotion = GetEditingMerchPromotionInfo(promotion.MerchantId);
            promotion.Id = edittingPromotion != null ? edittingPromotion.Id : Guid.Empty;
            MerchantPromotionView dbPromotion = edittingPromotion != null ? edittingPromotion : null;

            var sourceCovers = promotion.Covers.Select(item => new MutiLanguage
            {
                Desc = item.Desc,
                Lang = new SystemLang { Id = item.Lang.Id, Code = item.Lang.Code, Text = item.Lang.Text }
            }).ToList();
            var sourceLogos = promotion.Logos.Select(item => new MutiLanguage
            {
                Desc = item.Desc,
                Lang = new SystemLang { Id = item.Lang.Id, Code = item.Lang.Code, Text = item.Lang.Text }
            }).ToList();
            var sourceBanners = promotion.Banners.Select(item => new MerchantPromotionBannerView
            {
                Id = item.Id,
                Image = item.Image,
                IsDelete = item.IsDelete,
                Lang = item.Lang,
                Language = item.Language,
                PromotionId = item.PromotionId,
                Seq = item.Seq
            }).ToList();

            //生成相关图片的相对路径或绝对路径
            GenPromotionImage(promotion, dbPromotion);

            if (dbPromotion == null)//如果不存在Editing的Promotion则将推广的产品复制一份
            {
                if (promotion.ProductList != null && promotion.ProductList.Any())                
                    foreach (var item in promotion.ProductList) item.Id = Guid.Empty;                                
            }
            bool flag = false;
            if (promotion.Id == Guid.Empty)           
                flag= InsertMerchantPromotion(promotion);           
            else
                flag = UpdateMerchantPromotion(promotion);

            //把相关图片从Temp目录移动到真实目录下
            if (flag)
               flag = CreatePromotionImage(sourceCovers, sourceLogos, sourceBanners, dbPromotion, promotion);

            return flag;
        }

        public MerchantPromotionView GetEditingMerchPromotionInfo(Guid merchID)
        {
            MerchantPromotionView view = new MerchantPromotionView();

            MerchantPromotion promotion = new MerchantPromotion();

            promotion = merchantPromotionRepository.GetNotApprovePromotion(merchID);

            if (promotion == null) return null;

            var fileServer = string.Empty;
            view.Id = promotion.Id;
            view.MerchantId = promotion.MerchantId;
            view.CoverId = promotion.CoverId; //fileServer + promotion.Cover + "?tid=" + (promotion.UpdateDate ?? DateTime.Now).ToString("ddmmss");
            view.MobileCoverId = promotion.MobileCoverId;
            view.LogoId = promotion.SmallLogoId; //fileServer + promotion.SmallLogo + "?tid=" + (promotion.UpdateDate ?? DateTime.Now).ToString("ddmmss");

            view.Covers = translationRepository.GetMutiLanguage(promotion.CoverId);
            view.MobileCovers = translationRepository.GetMutiLanguage(promotion.MobileCoverId);
            view.Logos = translationRepository.GetMutiLanguage(promotion.SmallLogoId);
            view.BigLogos = translationRepository.GetMutiLanguage(promotion.BigLogoId);

            view.Banners =baseRepository.GetList<MerchantPromotionBanner>(b=> !b.IsDeleted && b.IsActive && b.PromotionId== promotion.Id)
                                    .Select(b => new MerchantPromotionBannerView
                                    {
                                        Id = b.Id,
                                        Image = fileServer + b.Image,
                                        IsDelete = false,
                                        PromotionId = b.PromotionId,
                                        Lang = b.Language,
                                        Language = b.Language.ToString(),
                                        Seq = b.Seq,
                                        IsOpenWindow = b.IsOpenWindow,
                                        BannerLink = b.BannerLink
                                    }).OrderBy(o => o.Lang).ThenBy(t => t.Seq).ToList();

            view.ProductList = (from b in baseRepository.GetList< MerchantPromotionProduct>(x=>x.PromotionId== promotion.Id)
                                join c in baseRepository.GetList<Product>() on b.ProductCode equals c.Code 
                                where !b.IsDeleted && b.IsActive && c.Status == ProductStatus.OnSale && !c.IsDeleted
                                select new MerchantPromotionProductView
                                {
                                    Id = b.Id,
                                    IsDelete = false,
                                    PromotionId = b.PromotionId,
                                    Code = b.ProductCode,
                                    ProductId = c.Id,
                                    NameTransId = c.NameTransId,
                                    Name = "",
                                    ListPrice = c.OriginalPrice,
                                    SalesPrice = c.SalePrice,
                                    CurrencyCode = c.CurrencyCode,
                                    MerchantId = c.MerchantId
                                }).ToList();

            if (view.ProductList != null)
            {
                foreach (var item in view.ProductList)
                {
                    item.Imgs = GetProductImages(item.ProductId);
                    item.Name = translationRepository.GetTranslation(item.NameTransId, CurrentUser.Lang).Value;
                    item.Currency = currencyBLL.GetSimpleCurrency(item.CurrencyCode);
                }
            }
            var introductions = translationRepository.GetMutiLanguage(promotion.IntorductionTranId);
            var descs = translationRepository.GetMutiLanguage(promotion.DescTransId);
            var names = translationRepository.GetMutiLanguage(promotion.NameTranId);
            var tandcs = translationRepository.GetMutiLanguage(promotion.TAndCTranId);
            var notices = translationRepository.GetMutiLanguage(promotion.NoticeTranId);
            var rtnTermses = translationRepository.GetMutiLanguage(promotion.ReturnTermsTranId);

            view.PromotionNames = names;
            view.Descriptions = descs;
            view.PromotionIntroductions = introductions;
            view.TAndCs = tandcs;
            view.Notices = notices;
            view.ReturnTermses = rtnTermses;

            view.PromotionIntroduction = introductions.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.PromotionName = names.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.Description = descs.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.TAndC = tandcs.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.Notice = notices.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.ReturnTerms = rtnTermses.Where(p => p.Language == CurrentUser.Lang).FirstOrDefault()?.Desc ?? "";
            view.OverSeaCoolDownDay = promotion.OverSeaCoolDownDay;
            view.LocalCoolDownDay = promotion.LocalCoolDownDay;

            return view;
        }

        public bool InsertMerchantPromotion(MerchantPromotionView promotion)
        {         
            if (promotion != null)
            {
                foreach (var item in promotion.Descriptions)
                {
                    item.Desc = StringUtil.FilterHTMLFunction(item.Desc);
                }
            }
            Guid introId = Guid.NewGuid(); 
            var introList  = translationRepository.GenTranslations(promotion.PromotionIntroductions, TranslationType.MerchantPromotion, introId);
            Guid descId = Guid.NewGuid();
            var descList = translationRepository.GenTranslations(promotion.Descriptions, TranslationType.MerchantPromotion, descId);
            Guid nameId = Guid.NewGuid();
            var nameList = translationRepository.GenTranslations(promotion.PromotionNames, TranslationType.MerchantPromotion, nameId);
            Guid tandcId = Guid.NewGuid();
            var tandcList = translationRepository.GenTranslations(promotion.TAndCs, TranslationType.MerchantPromotion, tandcId);
            Guid noticeId = Guid.NewGuid();
            var noticeList = translationRepository.GenTranslations(promotion.Notices, TranslationType.MerchantPromotion, noticeId);
            Guid rtnTermsId = Guid.NewGuid();
            var rntTermList = translationRepository.GenTranslations(promotion.ReturnTermses, TranslationType.MerchantPromotion, rtnTermsId);
            Guid orderTranId = Guid.NewGuid();
            var orderTranList  = translationRepository.GenTranslations(promotion.ExpCompleteDays, TranslationType.MerchantPromotion, orderTranId);

            MerchantPromotion promo = new MerchantPromotion();
            promo.Id = Guid.NewGuid();

            promo.CoverId = Guid.NewGuid();
            var coverList  = translationRepository.GenTranslations(promotion.Covers, TranslationType.MerchantPromotion,promo.CoverId);// relativePath + "/" + coverName;
            promo.MobileCoverId = Guid.NewGuid();
            var mobCoverList = translationRepository.GenTranslations(promotion.MobileCovers, TranslationType.MerchantPromotion, promo.MobileCoverId);
            promo.SmallLogoId = Guid.NewGuid();
            var smallLogoList = translationRepository.GenTranslations(promotion.Logos, TranslationType.MerchantPromotion, promo.SmallLogoId); //relativePath + "/" + smallLogo;
            promo.BigLogoId = Guid.NewGuid();
            var bigLogoList= translationRepository.GenTranslations(promotion.BigLogos, TranslationType.MerchantPromotion, promo.BigLogoId); //relativePath + "/" + bigLogo;

            promo.MerchantId = promotion.MerchantId;
            promo.NameTranId = nameId;
            promo.DescTransId = descId;
            promo.TAndCTranId = tandcId;
            promo.NoticeTranId = noticeId;
            promo.ReturnTermsTranId = rtnTermsId;
            promo.IntorductionTranId = introId;
            promo.ApproveStatus = ApproveType.Editing;
            promo.OrderTransId = orderTranId;
            promo.OverSeaCoolDownDay = promotion.OverSeaCoolDownDay;
            promo.LocalCoolDownDay = promotion.LocalCoolDownDay;
            //promo.IsActive = false;

            promotion.Id = promo.Id;

            var bannerCollection = InsertAndDeletePromotionBanner(promotion);
            var productCollection = InsertAndDeletePromotionProduct(promotion);
            bool flag = false;
            using var tran = baseRepository.CreateTransation();
            baseRepository.Insert(promo);

            baseRepository.Insert(introList);
            baseRepository.Insert(descList);
            baseRepository.Insert(nameList);
            baseRepository.Insert(tandcList);
            baseRepository.Insert(noticeList);
            baseRepository.Insert(rntTermList);
            baseRepository.Insert(orderTranList);
            baseRepository.Insert(coverList);
            baseRepository.Insert(mobCoverList);
            baseRepository.Insert(smallLogoList);
            baseRepository.Insert(bigLogoList);

            baseRepository.Delete(bannerCollection.Item1);
            baseRepository.Insert(bannerCollection.Item2);
            baseRepository.Update(bannerCollection.Item3);

            baseRepository.Delete(productCollection.Item1);
            baseRepository.Insert(productCollection.Item2);

            tran.Commit();         
            flag = true;
            return flag;
        }

        public bool UpdateMerchantPromotion(MerchantPromotionView promotion)
        {
            bool flag = false;
            var promo = merchantPromotionRepository.GetNotApprovePromotion(promotion.MerchantId);
            if (promo != null)
            {
                var smallLogoList = translationRepository.GenTranslations(promotion.Logos, TranslationType.MerchantPromotion, promo.SmallLogoId, ActionTypeEnum.Modify); 
                var bigLogoList = translationRepository.GenTranslations(promotion.BigLogos, TranslationType.MerchantPromotion, promo.BigLogoId, ActionTypeEnum.Modify);
                var coverList = translationRepository.GenTranslations( promotion.Covers, TranslationType.MerchantPromotion, promo.CoverId, ActionTypeEnum.Modify);
                var mobCoverList = translationRepository.GenTranslations(promotion.MobileCovers, TranslationType.MerchantPromotion, promo.MobileCoverId, ActionTypeEnum.Modify);
                var introList = translationRepository.GenTranslations(promotion.PromotionIntroductions, TranslationType.MerchantPromotion, promo.IntorductionTranId,ActionTypeEnum.Modify);
                var descList  = translationRepository.GenTranslations(promotion.Descriptions, TranslationType.MerchantPromotion, promo.DescTransId, ActionTypeEnum.Modify);
                var nameList = translationRepository.GenTranslations( promotion.PromotionNames, TranslationType.MerchantPromotion, promo.NameTranId, ActionTypeEnum.Modify);
                var tandcList = translationRepository.GenTranslations( promotion.TAndCs, TranslationType.MerchantPromotion, promo.TAndCTranId,ActionTypeEnum.Modify);
                var noticeList = translationRepository.GenTranslations(promotion.Notices, TranslationType.MerchantPromotion, promo.NoticeTranId, ActionTypeEnum.Modify);
                var rntTermList = translationRepository.GenTranslations(promotion.ReturnTermses, TranslationType.MerchantPromotion, promo.ReturnTermsTranId, ActionTypeEnum.Modify);
                var orderTranList = translationRepository.GenTranslations(promotion.ExpCompleteDays, TranslationType.MerchantPromotion, promo.OrderTransId, ActionTypeEnum.Modify);

                promo.LocalCoolDownDay = promotion.LocalCoolDownDay;
                promo.OverSeaCoolDownDay = promotion.OverSeaCoolDownDay;
                promo.UpdateDate = DateTime.Now;
                promo.ApproveStatus = ApproveType.Editing;
                promotion.Id = promo.Id;

               var bannerCollection= InsertAndDeletePromotionBanner(promotion);
               var productCollection = InsertAndDeletePromotionProduct(promotion);
               
                using var tran = baseRepository.CreateTransation();
                baseRepository.Update(promo);

                baseRepository.Update(introList);
                baseRepository.Update(descList);
                baseRepository.Update(nameList);
                baseRepository.Update(tandcList);
                baseRepository.Update(noticeList);
                baseRepository.Update(rntTermList);
                baseRepository.Update(orderTranList);
                baseRepository.Update(coverList);
                baseRepository.Update(mobCoverList);
                baseRepository.Update(smallLogoList);
                baseRepository.Update(bigLogoList);

                baseRepository.Delete(bannerCollection.Item1);
                baseRepository.Insert(bannerCollection.Item2);
                baseRepository.Update(bannerCollection.Item3);

                baseRepository.Delete(productCollection.Item1);
                baseRepository.Insert(productCollection.Item2);

                tran.Commit();
                flag = true;
            }
            else
            {
                flag =InsertMerchantPromotion(promotion);
            }
            return flag;
        }

        public SystemResult ApplyApprove(Guid id)
        {
            SystemResult result = new SystemResult();
            var merchPromotion = merchantPromotionRepository.GetNotApprovePromotion(id);

            if (merchPromotion != null)
            {
                merchPromotion.ApproveStatus = ApproveType.WaitingApprove;
                merchPromotion.UpdateDate = DateTime.Now;
                var appHistory = InsertMerchantApproveHistory(id, ApproveResult.WaitingApprove, "");

                using var tran = baseRepository.CreateTransation();
                baseRepository.Update(merchPromotion);
                baseRepository.Insert(appHistory);

                tran.Commit();
            }

            result.Succeeded = true;

            return result;
        }

        public async Task<SystemResult> ApproveMerchantAsync(List<string> ids)
        {
            SystemResult sysRslt = await ApproveMerchant(ids);
            if (sysRslt.Succeeded)
            {               
                foreach (var idStr in ids)
                {                  
                    await UpdateCache(Guid.Parse(idStr));
                }
            }
            return sysRslt;
        }

        public async Task<SystemResult> RejectMerchant(Guid merchId, string reason)
        {
            SystemResult result = new SystemResult();

            var notApproveMerchantPromotion = merchantPromotionRepository.GetNotApprovePromotion(merchId);
            if (notApproveMerchantPromotion != null)
            {
                notApproveMerchantPromotion.ApproveStatus = ApproveType.Reject;
                notApproveMerchantPromotion.UpdateDate = DateTime.Now;
                notApproveMerchantPromotion.UpdateBy = Guid.Parse(CurrentUser.UserId);

                var merchInfo = baseRepository.GetModelById<Merchant>(notApproveMerchantPromotion.MerchantId);
                if (merchInfo != null)
                {
                    //RecipientInfo info = new RecipientInfo();
                    //info.Id = merchInfo.Id;
                    //info.Code = merchInfo.MerchNo;
                    //info.Email = merchInfo.ContactEmail;
                    //info.Lang = merchInfo.Language;
                    //info.Mobile = merchInfo?.ContactPhoneNum;
                    //info.Name = TranslationRepo.Entities.FirstOrDefault(d => d.TransId == merchInfo.NameTransId && d.Lang == info.Lang)?.Value ?? "";
                    //ContentAppr model = new ContentAppr();
                    //model.CreateDate = notApproveMerchantPromotion.UpdateDate ?? notApproveMerchantPromotion.CreateDate;
                    //model.StatusName = Resources.Value.Reject;
                    //model.Reason = reason;
                    //MessageBLL.SendMerchPromApproveToMerch(info, model);

                    //生成商家消息，待完成
                }
               
                var appHistory= InsertMerchantApproveHistory(merchId, ApproveResult.Reject, reason);
                using var tran = baseRepository.CreateTransation();

                baseRepository.Update(notApproveMerchantPromotion);
                baseRepository.Insert(appHistory);
                tran.Commit();
                result.Succeeded = true;

                if (result.Succeeded)
                { 
                    //发送消息通过商家，待完成
                }
            }

            result.Succeeded = true;
            return result;
        }

        public ApproveHistory InsertMerchantApproveHistory(Guid merchId, ApproveResult type, string remark)
        {
            ApproveHistory obj = new ApproveHistory();

            obj.Id = Guid.NewGuid();
            obj.ApproveType = type;
            obj.MerchantId = merchId;
            obj.OperateDate = DateTime.Now;
            obj.OperateId = Guid.Parse(CurrentUser.UserId);
            obj.ModuleId = merchId;
            obj.ApproveModule = ApproveModule.MerchantPromotion;
            obj.Remark = remark;

            return obj;
        }

        private MerchantShipMethodMappingView GenMerchantShipMethodView(List<MerchantActiveShipMethodDto> defaultShipMethod, List<MerchantActiveShipMethodDto> shipMethods)
        {
            MerchantShipMethodMappingView view = new MerchantShipMethodMappingView();
            if (shipMethods != null && shipMethods.Any())
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

            var ECShipInfo = new MerchantECShipInfo
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

            var NameList = translationRepository.GenTranslations(recUpdate.NameList, TranslationType.Merchant, recUpdate.NameTransId, ActionTypeEnum.Modify);
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
            var insertRole = new List<UserRole>();
            insertRole.Add(userRole);
            var deleteRole = new List<UserRole>();
            foreach (var item in userRoles)
            {
                if (item.RoleId == extRoleId || item.RoleId == extGS1RoleId || item.RoleId == intRoleId || item.RoleId == intGS1RoleId)
                    deleteRole.Add(item);
            }

            var result = new Tuple<List<UserRole>, List<UserRole>>(deleteRole, insertRole);
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

        /// <summary>
        /// Item1是delbanners，Item2是insertBanners，Item3是updateBanners
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        private Tuple<List<MerchantPromotionBanner>, List<MerchantPromotionBanner>, List<MerchantPromotionBanner>> InsertAndDeletePromotionBanner(MerchantPromotionView promotion)
        {
            var delIds = promotion.Banners.Where(d => d.IsDelete && d.Id != Guid.Empty).Select(s => s.Id).ToList();
            var delbanners = baseRepository.GetList<MerchantPromotionBanner>(x => delIds.Contains(x.Id)).ToList();

            var insertBanners = (from d in promotion.Banners
                                 where d.IsDelete == false
                                 && d.Id == Guid.Empty
                                 select new MerchantPromotionBanner
                                 {
                                     Id = Guid.NewGuid(),
                                     Image = d.Image,
                                     PromotionId = promotion.Id,
                                     //Language = (Language)Enum.Parse(typeof(Language), d.Language),
                                     Language = d.Language.ToEnum<Language>(),
                                     Seq = d.Seq,
                                     BannerLink = d.BannerLink,
                                     IsOpenWindow = d.IsOpenWindow
                                 }).ToList();

            var updateBanners = (from d in promotion.Banners
                                 where d.IsDelete == false
                                 && d.Id != Guid.Empty
                                 select new MerchantPromotionBanner
                                 {
                                     Id = d.Id,
                                     Image = d.Image,
                                     PromotionId = promotion.Id,
                                     Language = d.Language.ToEnum<Language>(),
                                     Seq = d.Seq,
                                     BannerLink = d.BannerLink,
                                     IsOpenWindow = d.IsOpenWindow
                                 }).ToList();

            var result = new Tuple<List<MerchantPromotionBanner>, List<MerchantPromotionBanner>, List<MerchantPromotionBanner>>(delbanners, insertBanners, updateBanners);
            return result;

        }

        /// <summary>
        /// Item1是delProducts，Item2是insertProducts
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        private Tuple<List<MerchantPromotionProduct>, List<MerchantPromotionProduct>> InsertAndDeletePromotionProduct(MerchantPromotionView promotion)
        {
            var delIds = promotion.ProductList.Where(x => x.IsDelete).Select(s => s.Id).ToList();
            var delProducts = baseRepository.GetList<MerchantPromotionProduct>(x => delIds.Contains(x.Id)).ToList();

            var insertProducts = (from d in promotion.ProductList where !d.IsDelete && d.Id == Guid.Empty
                                  select new MerchantPromotionProduct
                                  {
                                      Id = Guid.NewGuid(),
                                      ProductCode = d.Code,
                                      PromotionId = promotion.Id,
                                      ProductId = d.ProductId
                                  }).ToList();

            var result = new Tuple<List<MerchantPromotionProduct>, List<MerchantPromotionProduct>>(delProducts, insertProducts);
            return result;
        }

        /// <summary>
        /// 生成图片目录路径
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="dbPromotion"></param>
        private void GenPromotionImage(MerchantPromotionView promotion, MerchantPromotionView dbPromotion)
        {
            var physicalPath = PathUtil.GetPhysicalPath(Configuration["UploadPath"], promotion.MerchantId.ToString(), FileFolderEnum.MerchantPromotion);
            var relativePath = PathUtil.GetRelativePath(promotion.MerchantId.ToString(), FileFolderEnum.MerchantPromotion);
            string tempPath = PathUtil.GetPhysicalPath(Configuration["UploadPath"], promotion.MerchantId.ToString(),FileFolderEnum.TempPath);

            if (dbPromotion != null)//存在編輯中、待審批的Promotion進行圖片更新
            {
                #region 
                //foreach (var cover in promotion.Covers)
                //{
                //    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(cover.Desc ?? ""))))
                //    {
                //        var extension = Path.GetExtension(cover.Desc);
                //        cover.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //    }
                //}

                //foreach (var mobilecovers in promotion.MobileCovers)
                //{
                //    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(mobilecovers.Desc ?? ""))))
                //    {
                //        var extension = Path.GetExtension(mobilecovers.Desc);
                //        mobilecovers.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //    }
                //}

                //foreach (var banner in promotion.Banners)
                //{
                //    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(banner.Image ?? ""))))
                //    {
                //        //var extension = Path.GetExtension(banner.Image);
                //        banner.Image = relativePath + "/" + Path.GetFileName(banner.Image);
                //    }
                //}

                //foreach (var logo in promotion.Logos)
                //{
                //    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(logo.Desc ?? ""))))
                //    {
                //        var extension = Path.GetExtension(logo.Desc);
                //        logo.Desc = relativePath + "/" + Guid.NewGuid() + extension;

                //        foreach (var bigLogo in promotion.BigLogos)
                //        {
                //            if (bigLogo.Language == logo.Language)
                //            {
                //                bigLogo.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //            }
                //        }
                //    }
                //}
                #endregion

                Action<MutiLanguage> CoversAction = (m) => {
                    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(m.Desc ?? ""))))
                    {
                        var extension = Path.GetExtension(m.Desc);
                        m.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                    }                  
                };
                SetPath(promotion, promotion.Covers,CoversAction);

                Action<MutiLanguage> MobileCoversAction = (m) => {
                    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(m.Desc ?? ""))))
                    {
                        var extension = Path.GetExtension(m.Desc);
                        m.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                    }
                };
                SetPath(promotion, promotion.MobileCovers, MobileCoversAction);

                Action<MerchantPromotionBannerView> BannersAction = (m) =>
                {
                    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(m.Image ?? ""))))
                    {
                        m.Image = relativePath + "/" + Path.GetFileName(m.Image);
                    }
                };
                SetPath(promotion, promotion.Banners, BannersAction);

                Action<MutiLanguage> LogoAction = (m) => {                  
                    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(m.Desc ?? ""))))
                    {
                        var extension = Path.GetExtension(m.Desc);
                        m.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                        SetLogoPath(promotion.BigLogos, m.Language, relativePath, extension);
                    }
                };
                SetPath(promotion, promotion.Logos, LogoAction);
            }
            else//沒有存在編輯中、待審批的Promotion，需要將原promotion的圖片複製，并保存新添加的圖片
            {
                #region 

                //foreach (var cover in promotion.Covers)
                //{
                //    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(cover.Desc ?? ""))))
                //    {
                //        var extension = Path.GetExtension(cover.Desc);
                //        cover.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //    }
                //    else//複製原promotion的圖片
                //    {
                //        var oldPath = physicalPath + "\\" + Path.GetFileName(cover.Desc ?? "");
                //        if (File.Exists(oldPath))
                //        {
                //            var extension = Path.GetExtension(oldPath);
                //            cover.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //        }
                //    }
                //}

                //foreach (var mobilecovers in promotion.MobileCovers)
                //{
                //    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(mobilecovers.Desc ?? ""))))
                //    {
                //        var extension = Path.GetExtension(mobilecovers.Desc);
                //        mobilecovers.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //    }
                //    else//複製原promotion的圖片
                //    {
                //        var oldPath = physicalPath + "\\" + Path.GetFileName(mobilecovers.Desc ?? "");
                //        if (File.Exists(oldPath))
                //        {
                //            var extension = Path.GetExtension(oldPath);
                //            mobilecovers.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //        }
                //    }
                //}

                //foreach (var logo in promotion.Logos)
                //{
                //    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(logo.Desc ?? ""))))
                //    {
                //        var extension = Path.GetExtension(logo.Desc);
                //        logo.Desc = relativePath + "/" + Guid.NewGuid() + extension;

                //        foreach (var bigLogo in promotion.BigLogos)
                //        {
                //            if (bigLogo.Language == logo.Language)
                //            {
                //                bigLogo.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //            }
                //        }
                //    }
                //    else//複製原promotion的圖片
                //    {
                //        var oldPath = physicalPath + "\\" + Path.GetFileName(logo.Desc ?? "");
                //        if (File.Exists(oldPath))
                //        {
                //            var extension = Path.GetExtension(oldPath);
                //            logo.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //            foreach (var bigLogo in promotion.BigLogos)
                //            {
                //                if (bigLogo.Language == logo.Language)
                //                {
                //                    bigLogo.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                //                }
                //            }
                //        }
                //    }
                //}

                //foreach (var banner in promotion.Banners)
                //{
                //    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(banner.Image ?? ""))))
                //    {
                //        //var extension = Path.GetExtension(banner.Image);
                //        banner.Image = relativePath + "/" + Guid.NewGuid() + Path.GetExtension(banner.Image);
                //    }
                //    else//複製原promotion的圖片
                //    {
                //        banner.Id = Guid.Empty;
                //        var oldPath = physicalPath + "\\" + Path.GetFileName(banner.Image ?? "");
                //        if (File.Exists(oldPath))
                //        {
                //            banner.Image = relativePath + "/" + Guid.NewGuid() + Path.GetExtension(banner.Image);

                //        }
                //    }
                //}
                #endregion

                Action<MutiLanguage> CoversAction = (m) => {
                    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(m.Desc ?? ""))))
                    {
                        var extension = Path.GetExtension(m.Desc);
                        m.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                    }
                    else {
                        var oldPath = physicalPath + "\\" + Path.GetFileName(m.Desc ?? "");
                        if (File.Exists(oldPath))
                        {
                            var extension = Path.GetExtension(oldPath);
                            m.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                        }
                    }
                };
                SetPath(promotion, promotion.Covers, CoversAction);

                Action<MutiLanguage> MobileCoversAction = (m) => {
                    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(m.Desc ?? ""))))
                    {
                        var extension = Path.GetExtension(m.Desc);
                        m.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                    }
                    else
                    {
                        var oldPath = physicalPath + "\\" + Path.GetFileName(m.Desc ?? "");
                        if (File.Exists(oldPath))
                        {
                            var extension = Path.GetExtension(oldPath);
                            m.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                        }
                    }
                };
                SetPath(promotion, promotion.MobileCovers, MobileCoversAction);

                Action<MerchantPromotionBannerView> BannersAction = (m) =>
                {
                    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(m.Image ?? ""))))
                    {
                        m.Image = relativePath + "/" + Path.GetFileName(m.Image);
                    }
                    else
                    {
                        m.Id = Guid.Empty;
                        var oldPath = physicalPath + "\\" + Path.GetFileName(m.Image ?? "");
                        if (File.Exists(oldPath))
                        {
                            m.Image = relativePath + "/" + Guid.NewGuid() + Path.GetExtension(m.Image);
                        }
                    }
                };
                SetPath(promotion, promotion.Banners, BannersAction);

                Action<MutiLanguage> LogoAction = (m) => {
                    if (File.Exists(Path.Combine(tempPath, Path.GetFileName(m.Desc ?? ""))))
                    {
                        var extension = Path.GetExtension(m.Desc);
                        m.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                        SetLogoPath(promotion.BigLogos, m.Language, relativePath, extension);
                    }
                    else
                    {
                        var oldPath = physicalPath + "\\" + Path.GetFileName(m.Desc ?? "");
                        if (File.Exists(oldPath))
                        {
                            var extension = Path.GetExtension(oldPath);
                            m.Desc = relativePath + "/" + Guid.NewGuid() + extension;                         
                            SetLogoPath(promotion.BigLogos, m.Language, relativePath, extension);
                        }
                    }
                };
                SetPath(promotion, promotion.Logos, LogoAction);
            }
        }

        /// <summary>
        /// 创建图片到对应路径
        /// </summary>
        /// <param name="sourceCovers"></param>
        /// <param name="sourceLogos"></param>
        /// <param name="sourceBanners"></param>
        /// <param name="dbPromotion"></param>
        /// <param name="promotion"></param>
        /// <returns></returns>
        private bool CreatePromotionImage(List<MutiLanguage> sourceCovers, List<MutiLanguage> sourceLogos, List<MerchantPromotionBannerView> sourceBanners, MerchantPromotionView dbPromotion, MerchantPromotionView promotion)
        {
            string tempPath = PathUtil.GetPhysicalPath(Configuration["UploadPath"], promotion.MerchantId.ToString(), FileFolderEnum.TempPath);
            string targerPath = PathUtil.GetPhysicalPath(Configuration["UploadPath"], promotion.MerchantId.ToString(), FileFolderEnum.MerchantPromotion);
            var relativePath = PathUtil.GetRelativePath( promotion.MerchantId.ToString(), FileFolderEnum.MerchantPromotion);

            var insertList = new List<string>();
            var deleteList = new List<string>();

            var imageSizes = settingBLL.GetProductImageSize();

            if (dbPromotion == null)
            {
                foreach (var cover in sourceCovers)
                {
                    if (!string.IsNullOrEmpty(cover?.Desc))
                    {
                        var tempFlie = Path.Combine(tempPath, Path.GetFileName(cover.Desc));
                        if (File.Exists(tempFlie))
                        {
                            var targetFile = Path.GetFileName(promotion.Covers.FirstOrDefault(p => p.Language == cover.Language)?.Desc);
                            FileUtil.MoveFile(tempFlie, targerPath, targetFile);
                            insertList.Add(relativePath + "/" + targetFile);
                        }
                        else
                        {
                            var oldPath = targerPath + "\\" + Path.GetFileName(cover.Desc);
                            if (File.Exists(oldPath))
                            {
                                var targetFile = Path.GetFileName(promotion.Covers.FirstOrDefault(p => p.Language == cover.Language)?.Desc);
                                FileUtil.CopyFile(oldPath, targerPath, targetFile);
                                insertList.Add(relativePath + "/" + targetFile);
                            }
                        }
                    }
                }

                foreach (var logo in sourceLogos)
                {
                    if (!string.IsNullOrEmpty(logo?.Desc))
                    {
                        var tempFlie = Path.Combine(tempPath, Path.GetFileName(logo.Desc));
                        if (File.Exists(tempFlie))
                        {
                            var smallImageFile = Path.GetFileName(promotion.Logos.FirstOrDefault(p => p.Language == logo.Language)?.Desc);
                            var bigImageFile = Path.GetFileName(promotion.BigLogos.FirstOrDefault(p => p.Language == logo.Language)?.Desc);
                            ImageUtil.CreateImg(tempFlie, targerPath, smallImageFile, imageSizes[1].Width, imageSizes[1].Length);//200*200
                            FileUtil.MoveFile(tempFlie, targerPath, bigImageFile);
                            insertList.Add(relativePath + "/" + smallImageFile);
                            insertList.Add(relativePath + "/" + bigImageFile);
                        }
                        else
                        {
                            var oldPath = targerPath + "\\" + Path.GetFileName(logo.Desc);
                            if (File.Exists(oldPath))
                            {
                                var smallImageFile = Path.GetFileName(promotion.Logos.FirstOrDefault(p => p.Language == logo.Language)?.Desc);
                                var bigImageFile = Path.GetFileName(promotion.BigLogos.FirstOrDefault(p => p.Language == logo.Language)?.Desc);

                                FileUtil.CopyFile(oldPath, targerPath, smallImageFile);
                                FileUtil.CopyFile(oldPath, targerPath, bigImageFile);
                                insertList.Add(relativePath + "/" + smallImageFile);
                                insertList.Add(relativePath + "/" + bigImageFile);
                            }
                        }
                    }

                }

                if (promotion.Banners != null)
                {
                    for (int i = 0; i < sourceBanners.Count; i++)
                    {
                        var sourceBanner = sourceBanners[i];
                        var tempFlie = Path.Combine(tempPath, Path.GetFileName(sourceBanner.Image));
                        var dbBanner = promotion.Banners[i];
                        if (File.Exists(tempFlie))
                        {
                            var targetFile = Path.GetFileName(dbBanner.Image);
                            FileUtil.MoveFile(tempFlie, targerPath, targetFile);
                            insertList.Add(relativePath + "/" + targetFile);
                        }
                        else
                        {
                            var oldPath = targerPath + "\\" + Path.GetFileName(sourceBanner.Image);
                            if (File.Exists(oldPath))
                            {
                                if (sourceBanner.IsDelete == false)
                                {

                                    var targetFile = Path.GetFileName(dbBanner.Image);
                                    FileUtil.CopyFile(oldPath, targerPath, targetFile);
                                    insertList.Add(relativePath + "/" + targetFile);
                                }

                            }
                        }
                    }
                }

            }
            else
            {
                foreach (var cover in sourceCovers)
                {
                    var tempFlie = Path.Combine(tempPath, Path.GetFileName(cover.Desc ?? ""));
                    if (File.Exists(tempFlie))
                    {
                        var oldFile = Path.GetFileName(dbPromotion?.Covers.FirstOrDefault(p => p.Language == cover.Language)?.Desc);
                        var targetFile = Path.GetFileName(promotion?.Covers.FirstOrDefault(p => p.Language == cover.Language)?.Desc);

                        FileUtil.DeleteFile(Path.Combine(targerPath, oldFile));
                        FileUtil.MoveFile(tempFlie, targerPath, targetFile);

                        insertList.Add(relativePath + "/" + targetFile);
                        deleteList.Add(relativePath + "/" + oldFile);
                    }
                }

                foreach (var logo in sourceLogos)
                {
                    var tempFlie = Path.Combine(tempPath, Path.GetFileName(logo.Desc ?? ""));
                    if (File.Exists(tempFlie))
                    {
                        var oldSmallFile = dbPromotion?.Logos == null ? "" : Path.GetFileName(dbPromotion?.Logos.FirstOrDefault(p => p.Language == logo.Language)?.Desc);
                        var oldBigFile = dbPromotion?.BigLogos == null ? "" : Path.GetFileName(dbPromotion?.BigLogos.FirstOrDefault(p => p.Language == logo.Language)?.Desc);

                        var smallImageFile = Path.GetFileName(promotion.Logos.FirstOrDefault(p => p.Language == logo.Language)?.Desc);
                        var bigImageFile = Path.GetFileName(promotion.BigLogos.FirstOrDefault(p => p.Language == logo.Language)?.Desc);

                        FileUtil.DeleteFile(Path.Combine(targerPath, oldSmallFile));
                        FileUtil.DeleteFile(Path.Combine(targerPath, oldBigFile));

                        ImageUtil.CreateImg(tempFlie, targerPath, smallImageFile, imageSizes[1].Width, imageSizes[1].Length);//200*200
                        //ImageUtil.CreateImg(tempFlie, targerPath, bigImageFile, imageSizes[7].Width, imageSizes[7].Length);//生成800*800的缩略图
                        FileUtil.MoveFile(tempFlie, targerPath, bigImageFile);

                        insertList.Add(relativePath + "/" + smallImageFile);
                        insertList.Add(relativePath + "/" + bigImageFile);
                        deleteList.Add(relativePath + "/" + oldSmallFile);
                        deleteList.Add(relativePath + "/" + oldBigFile);
                    }
                }

                if (promotion.Banners != null && promotion.Banners.Any())
                {
                    var insertImages = promotion.Banners.Where(p => p.Id == Guid.Empty).ToList();
                    var delImages = promotion.Banners.Where(p => p.Id != Guid.Empty && p.IsDelete == true).ToList();
                    foreach (var banner in insertImages)
                    {
                        var tempFlie = Path.Combine(tempPath, Path.GetFileName(banner.Image));
                        if (File.Exists(tempFlie))
                        {
                            var targetFile = Path.GetFileName(banner.Image);
                            FileUtil.MoveFile(tempFlie, targerPath, targetFile);
                            insertList.Add(relativePath + "/" + targetFile);
                        }
                    }

                    foreach (var dBanner in delImages)
                    {
                        var targetFile = Path.GetFileName(dBanner.Image);
                        FileUtil.DeleteFile(Path.Combine(targerPath, targetFile));
                        deleteList.Add(relativePath + "/" + targetFile);
                    }
                }
            }
      
            return true;
        }

        void SetPath(MerchantPromotionView promotion, List<MutiLanguage> list, Action<MutiLanguage> action)
        {            
            list.ForEach(x =>
            {                      
                    action.Invoke(x);           
            });
        }

        void SetPath(MerchantPromotionView promotion, List<MerchantPromotionBannerView> list, Action<MerchantPromotionBannerView> action)
        {          
            list.ForEach(x =>
            {               
                action.Invoke(x);                       
            });
        }

        void SetLogoPath(List<MutiLanguage> list,Language lang,string relativePath,string extension)
        {
            foreach (var item in list)
            {
                if (item.Language == lang)
                {
                    item.Desc = relativePath + "/" + Guid.NewGuid() + extension;
                }
            }
        }

        private async Task<SystemResult> ApproveMerchant(List<string> ids)
        {
            SystemResult result = new SystemResult();

            var notApproveList = new List<MerchantPromotion>(); 
            var hisActiveMerchList = new List<MerchantPromotion>();
            var appHistoryList  =new List<ApproveHistory>();
            foreach (var item in ids)
            {
                var merchantId = Guid.Parse(item);
                var notApproveMerchantPromotion = merchantPromotionRepository.GetNotApprovePromotion(merchantId);
                var activeMerchPromotion = merchantPromotionRepository.GetApprovePromotion(merchantId);
                if (notApproveMerchantPromotion != null)
                {
                    notApproveMerchantPromotion.ApproveStatus = ApproveType.Pass;           //审批为通过
                    notApproveMerchantPromotion.UpdateDate = DateTime.Now;
                    notApproveMerchantPromotion.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    notApproveList.Add(notApproveMerchantPromotion);

                    var appHistory = InsertMerchantApproveHistory(Guid.Parse(item), ApproveResult.Pass, "");
                    appHistoryList.Add(appHistory);

                    var merchInfo = baseRepository.GetModelById<Merchant>(notApproveMerchantPromotion.MerchantId);
                    if (merchInfo != null)
                    {
                        //RecipientInfo info = new RecipientInfo();
                        //info.Id = merchInfo.Id;
                        //info.Code = merchInfo.MerchNo;
                        //info.Email = merchInfo.ContactEmail;
                        //info.Lang = merchInfo.Language;
                        //info.Mobile = merchInfo.ContactPhoneNum;
                        //info.Name = TranslationRepo.Entities.FirstOrDefault(d => d.TransId == merchInfo.NameTransId && d.Lang == info.Lang)?.Value ?? "";
                        //ContentAppr model = new ContentAppr();
                        //model.CreateDate = notApproveMerchantPromotion.UpdateDate ?? notApproveMerchantPromotion.CreateDate;
                        //model.StatusName = Resources.Value.Pass;
                        //model.Reason = "";
                        //MessageBLL.SendMerchPromApproveToMerch(info, model);

                        //生成审批通过的消息,待完成

                    }                     
                }

                if (activeMerchPromotion != null)
                {
                    //历史的isActive =false
                    activeMerchPromotion.IsActive = false;
                    activeMerchPromotion.UpdateDate = DateTime.Now;
                    activeMerchPromotion.UpdateBy = Guid.Parse(CurrentUser.UserId);                  
                    hisActiveMerchList.Add(activeMerchPromotion);
                }
            }
          
            using var tran = baseRepository.CreateTransation();
            baseRepository.Insert(appHistoryList);
            baseRepository.Update(notApproveList);
            baseRepository.Update(hisActiveMerchList);
            result.Succeeded = true;
            tran.Commit();

            if (result.Succeeded)
            { 
                //发送消息通知商家，审批通过
            }

            result.Succeeded = true;
            return result;
        }
    }
}

