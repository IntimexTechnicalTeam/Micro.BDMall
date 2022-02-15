using AutoMapper;
using BDMall.Model;
using BDMall.BLL;
using BDMall.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Framework;
using BDMall.Repository;
using BDMall.Enums;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class MemberBLL : BaseBLL, IMemberBLL
    {
        IProductBLL productBLL;
        PreHeatFavoriteService preHeatFavoriteService;
        PreHeatProductService preHeatProductService;
        PreHeatMerchantService preHeatMerchantService;

        public MemberBLL(IServiceProvider services) : base(services)
        {
            productBLL = this.Services.Resolve<IProductBLL>();
            preHeatFavoriteService = (PreHeatFavoriteService)Services.GetService(typeof(PreHeatFavoriteService));
            preHeatProductService = (PreHeatProductService)Services.GetService(typeof(PreHeatProductService));
            preHeatMerchantService = (PreHeatMerchantService)Services.GetService(typeof(PreHeatMerchantService));
        }

        public PageData<MemberDto> SearchMember(MbrSearchCond cond)
        {
            var result = new PageData<MemberDto>();
            var query = baseRepository.GetList<Member>();

            #region 组装条件

            if (!cond.EmailAdd.IsEmpty())
                query = query.Where(x => x.Email.Contains(cond.EmailAdd));

            if (!cond.FirstName.IsEmpty())
                query = query.Where(x => x.FirstName.Contains(cond.FirstName));

            if (!cond.Code.IsEmpty())
                query = query.Where(x => x.Code.Contains(cond.Code));

            if (cond.RegDateFrom != null && cond.RegDateTo != null)
            {
                query = query.Where(x => x.CreateDate >= cond.RegDateFrom && x.CreateDate <= cond.RegDateTo);
            }
            #endregion

            result.TotalRecord = query.Count();

            result.Data = query.MapToList<Member, MemberDto>();

            return result;
        }

        public SystemResult Register(RegisterMember member)
        {
            var result = new SystemResult();

            member.Validate();

            var dbModel = new Member {

                Id = Guid.NewGuid(),
                Account = member.Account,
                Email = member.Email,
                Password = ToolUtil.Md5Encrypt(member.Password),
                IsActive = true, IsApprove = true, IsDeleted = false,
                CreateDate = DateTime.Now, UpdateDate = DateTime.Now,
                CurrencyCode = "HKD", Language = Language.C, BirthDate = member.BirthDate,
                Code = AutoGenerateNumber("MB"),
                FirstName = member.FirstName, LastName = member.LastName,
                OptOutPromotion = member.OptOutPromotion,
            };

            baseRepository.Insert(dbModel);

            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> ChangeLang(CurrentUser currentUser, Language Lang)
        {
            var result = new SystemResult() { Succeeded = false };

            var member = await baseRepository.GetModelByIdAsync<Member>(Guid.Parse(currentUser.UserId));
            member.Language = Lang;

            if (currentUser.IsLogin)
                await baseRepository.UpdateAsync(member);

            var newToken = jwtToken.RefreshToken(CurrentUser.Token, Lang, "");

            result.ReturnValue = newToken;
            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> ChangeCurrencyCode(CurrentUser currentUser, string CurrencyCode)
        {
            var result = new SystemResult() { Succeeded = false };

            var member = await baseRepository.GetModelByIdAsync<Member>(Guid.Parse(currentUser.UserId));
            member.CurrencyCode = CurrencyCode;

            if (currentUser.IsLogin)
                await baseRepository.UpdateAsync(member);

            var newToken = jwtToken.RefreshToken(CurrentUser.Token, null, CurrencyCode);

            result.ReturnValue = newToken;
            result.Succeeded = true;
            return result;
        }

        public RegSummary GetRegSummary()
        {
            RegSummary summary = new RegSummary();

            DateTime now = DateTime.Now;
            DateTime preMonth = (new DateTime(now.Year, now.Month, 1)).AddMonths(-1);
            DateTime d1 = new DateTime(now.Year, now.Month, 1);

            summary.MemberTotal = baseRepository.GetList<Member>().Count(d => !d.IsDeleted);
            summary.LastMth = baseRepository.GetList<Member>().Count(d => d.CreateDate.Month == preMonth.Month && !d.IsDeleted);
            summary.ThisMth = baseRepository.GetList<Member>().Count(d => d.CreateDate > d1 && d.CreateDate < DateTime.Now && !d.IsDeleted);
            return summary;
        }

        public async Task<SystemResult> AddFavMerchant(string merchCode)
        {
            var result = new SystemResult();
            var mch = await baseRepository.GetModelAsync<Merchant>(x => x.MerchNo == merchCode);
            if (mch == null) throw new BLException(Resources.Message.AddFavoriteFail);

            var merchFavEntity = await baseRepository.GetModelAsync<MerchantFavorite>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.MerchId == mch.Id);
            if (merchFavEntity == null)
            {
                merchFavEntity = new MerchantFavorite
                {
                    Id = Guid.NewGuid(),
                    MemberId = Guid.Parse(CurrentUser.UserId),
                    MerchId = mch.Id
                };
                await baseRepository.InsertAsync(merchFavEntity);
            }
            else
            {
                merchFavEntity.IsActive = true;
                await baseRepository.UpdateAsync(merchFavEntity);
            }

            //更新缓存
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);
            cacheData.MchList.Add(mch.Id);
            await RedisHelper.HSetAsync(key, field, cacheData);

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            result.Message = Resources.Message.AddFavoriteSuccess;
            return result;
        }

        public async Task<SystemResult> RemoveFavMerchant(string merchCode)
        {
            var result = new SystemResult();
            var mch = await baseRepository.GetModelAsync<Merchant>(x => x.MerchNo == merchCode);
            if (mch == null) throw new BLException(Resources.Message.AddFavoriteFail);

            var merchFavEntity = await baseRepository.GetModelAsync<MerchantFavorite>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.MerchId == mch.Id && x.IsActive);
            if (merchFavEntity != null)
            {
                merchFavEntity.IsActive = false;
                await baseRepository.UpdateAsync(merchFavEntity);
            }

            //更新缓存
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);
            cacheData.MchList.Remove(mch.Id);
            await RedisHelper.HSetAsync(key, field, cacheData);

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.RemoveFavoriteSuccess;
            return result;
        }

        public async Task<SystemResult> AddFavProduct(Guid productId)
        {
            var result = new SystemResult();
            var product = await baseRepository.GetModelByIdAsync<Product>(productId);
            if (product == null) throw new BLException(BDMall.Resources.Message.ProductCodeEmpty);

            var memberFavorite = await baseRepository.GetModelAsync<MemberFavorite>(d => d.ProductCode == product.Code && d.MemberId == Guid.Parse(CurrentUser.UserId));
            if (memberFavorite == null)
            {
                memberFavorite = new MemberFavorite();
                memberFavorite.Id = Guid.NewGuid();
                memberFavorite.MemberId = Guid.Parse(CurrentUser.UserId);
                memberFavorite.ProductId = product.Id;
                memberFavorite.ProductCode = product.Code;
                await baseRepository.InsertAsync(memberFavorite);
            }
            else
            {
                memberFavorite.IsActive = true;
                await baseRepository.UpdateAsync(memberFavorite);
            }

            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);
            cacheData.ProductList.Add(product.Code);
            await RedisHelper.HSetAsync(key, field, cacheData);

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.AddFavoriteSuccess;

            return result;
        }

        public async Task<SystemResult> RemoveFavProduct(Guid productId)
        {
            var result = new SystemResult();
            var product = await baseRepository.GetModelByIdAsync<Product>(productId);
            if (product == null) throw new BLException(BDMall.Resources.Message.ProductCodeEmpty);

            var memberFavorite = await baseRepository.GetModelAsync<MemberFavorite>(d => d.ProductCode == product.Code && d.MemberId == Guid.Parse(CurrentUser.UserId) && d.IsActive);
            if (memberFavorite != null)
            {
                memberFavorite.IsActive = false;
                await baseRepository.UpdateAsync(memberFavorite);
            }

            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);
            cacheData.ProductList.Remove(product.Code);
            await RedisHelper.HSetAsync(key, field, cacheData);

            result.ReturnValue = cacheData;
            result.Succeeded = true;
            result.Message = Resources.Message.RemoveFavoriteSuccess;

            return result;
        }

        public async Task<PageData<MicroMerchant>> MyFavMerchant(FavoriteCond cond)
        {
            var result = new PageData<MicroMerchant>();
            //cond.FavoriteType = FavoriteType.Merchant;
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;

            var favData = await RedisHelper.HGetAsync<Favorite>(key, field);
            if (favData == null || (favData.ProductList?.Any() ?? false))
            {
                //重新读取喜欢商家数据
                favData = await this.preHeatFavoriteService.GetDataSourceAsync(Guid.Parse(field));
                if (favData != null)
                {
                    //重新刷新缓存
                    await preHeatFavoriteService.SetDataToHashCache(Guid.Parse(CurrentUser.UserId), favData);
                }
            }

            key = $"{PreHotType.Hot_Merchants}_{CurrentUser.Lang}";
            var mchList = await RedisHelper.HGetAllAsync<HotMerchant>(key);
            //读数据库，并回写缓存           
            if ((!mchList?.Keys.Any() ?? false) || (!mchList.Values?.Any() ?? false))
            {
                var view = await this.preHeatMerchantService.GetDataSourceAsync(Guid.Empty);
                if (view != null && view.Any())
                {
                    //重新刷新缓存
                    await preHeatMerchantService.SetDataToHashCache(view, CurrentUser.Lang);
                    mchList = view.Where(x => x.LangType == CurrentUser.Lang).ToDictionary(x => x.MchId.ToString());
                }
            }

            var query = mchList.Values.AsQueryable().Where(x => favData.MchList.Contains(x.MchId));

            result.TotalRecord = query.Count();
            query = query.Skip(cond.Offset).Take(cond.PageSize);

            var list = query.Select(s => new MicroMerchant
            {
                Id = s.MchId,
                Code = s.Code,
                Name = s.Name,
                IsFavorite = true,

            }).ToList();

            foreach (var item in list)
            {
                var mp = await baseRepository.GetModelAsync<MerchantPromotion>(x => x.MerchantId == item.Id && x.IsActive && !x.IsDeleted && x.ApproveStatus == ApproveType.Pass);
                var mRecord = await preHeatMerchantService.DicCollection(mp, CurrentUser.Lang);
                item.ImagePath = mRecord["SmallLogoId"]?.Value ?? "";
            }
            result.Data = list;
            return result;
        }

        public async Task<PageData<MicroProduct>> MyFavProduct(FavoriteCond cond)
        {
            var result = new PageData<MicroProduct>();
            //cond.FavoriteType = FavoriteType.Product;
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;

            var favData = await RedisHelper.HGetAsync<Favorite>(key, field);
            if (favData == null || (favData.ProductList?.Any() ?? false))
            {
                //重新读取喜欢商家数据
                favData = await this.preHeatFavoriteService.GetDataSourceAsync(Guid.Parse(field));
                if (favData != null)
                {
                    //重新刷新缓存
                    await preHeatFavoriteService.SetDataToHashCache(Guid.Parse(CurrentUser.UserId), favData);
                }
            }

            key = $"{PreHotType.Hot_Products}_{CurrentUser.Lang}";
            var productList = await RedisHelper.HGetAllAsync<HotProduct>(key);
            //读数据库，并回写缓存           
            if ((!productList?.Keys.Any() ?? false) || (!productList.Values?.Any() ?? false))
            {
                var view = await this.preHeatProductService.GetDataSourceAsync(Guid.Empty);
                if (view != null && view.Any())
                {
                    //重新刷新缓存
                    await preHeatProductService.SetDataToHashCache(view, CurrentUser.Lang);
                    productList = view.Where(x => x.LangType == CurrentUser.Lang).ToDictionary(x => x.ProductId.ToString());
                }
            }

            var query = productList.Values.AsQueryable().Where(x => favData.ProductList.Contains(x.ProductCode));

            result.TotalRecord = query.Count();
            query = query.Skip(cond.Offset).Take(cond.PageSize);

            var list = query.Select(s => new MicroProduct
            {
                ProductId = s.ProductId,
                Code = s.Code,
                Name = s.Name,
                SalePrice = s.SalePrice,
                CurrencyCode = s.CurrencyCode,
                Score = s.Score,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                IsFavorite = true

            }).ToList();

            foreach (var item in list)
            {
                item.ImagePath = (await productBLL.GetProductImages(item.ProductId, item.Code)).FirstOrDefault();
            }
            result.Data = list;

            return result;
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentUser<MemberUser>> GetMemberInfo()
        {
            var member = await baseRepository.GetModelAsync<MemberAccount>(x => x.MemberId == Guid.Parse(CurrentUser.UserId));
            var memberData = new CurrentUser<MemberUser>
            {
                Account = CurrentUser.Account,
                CurrencyCode = CurrentUser.CurrencyCode,
                Email = CurrentUser.Email,
                IsLogin = CurrentUser.IsLogin,
                Lang = CurrentUser.Lang,
                LoginType = CurrentUser.LoginType,
                MerchantId = CurrentUser.MerchantId,
                Roles = null,
                Token = "",
                UserId = "",
                UserData = new MemberUser
                {
                    Fun = member.Fun,
                    MaxLimitDayFun = member.MaxLimitDayFun,
                    MaxLimitMonthFun = member.MaxLimitMonthFun,
                    MaxLimitYearFun = member.MaxLimitYearFun,
                    TotalDayFun = member.TotalDayFun,
                    TotalMonthFun = member.TotalMonthFun,
                    TotalYearFun = member.TotalYearFun,
                }
            };

            return memberData;
        }

        public async Task<PageData<MicroProduct>> MyProductTrack(TrackCond cond)
        {
            var result = new PageData<MicroProduct>();

            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;

            var favData = await RedisHelper.HGetAsync<Favorite>(key, field);
            if (favData == null || (favData.ProductList?.Any() ?? false))
            {
                //重新读取喜欢商家数据
                favData = await this.preHeatFavoriteService.GetDataSourceAsync(Guid.Parse(field));
                if (favData != null)
                {
                    //重新刷新缓存
                    await preHeatFavoriteService.SetDataToHashCache(Guid.Parse(CurrentUser.UserId), favData);
                }
            }

            var query = (await baseRepository.GetListAsync<ProductTrack>(x => x.MemberId == Guid.Parse(CurrentUser.UserId) && x.IsActive && !x.IsDeleted));

            key = $"{PreHotType.Hot_Products}_{CurrentUser.Lang}";
            var productList = (await RedisHelper.HMGetAsync<HotProduct>(key, query.Select(s => s.ProductCode).ToArray()))
                .Select(s => new MicroProduct
                {
                    ProductId = s.ProductId,
                    Code = s.Code,
                    Name = s.Name,
                    SalePrice = s.SalePrice,
                    CurrencyCode = s.CurrencyCode,
                    Score = s.Score,
                    IsFavorite = favData.ProductList.Any(x=>x == s.Code),
                    CreateDate = query.FirstOrDefault(x => x.ProductCode == s.Code).CreateDate,
                    UpdateDate = s.UpdateDate,

                }).OrderByDescending(o => o.CreateDate).ToList();

            if (productList?.Any() ?? false)
            { 
                //从数据库读
            }

            result.TotalRecord = productList.Count();
            result.Data = productList.Skip(cond.Offset).Take(cond.PageSize).ToList();

            foreach (var item in result.Data)
            {
                item.ImagePath = (await productBLL.GetProductImages(item.ProductId, item.Code)).FirstOrDefault();
            }

            return result;
        }
    }
}
