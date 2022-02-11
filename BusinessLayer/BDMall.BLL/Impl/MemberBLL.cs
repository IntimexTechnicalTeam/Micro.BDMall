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
        IMediator mediatR;
        IProductRepository productRepository;
        public MemberBLL(IServiceProvider services) : base(services)
        {
            mediatR = this.Services.Resolve<IMediator>();
            productRepository = this.Services.Resolve<IProductRepository>();
        }

        public PageData<MemberDto> SearchMember(MbrSearchCond cond)
        {

            var testuser = CurrentUser;

            var result = new PageData<MemberDto>();
            var query = baseRepository.GetList<Member>();

            #region 组装条件

            if (!cond.EmailAdd.IsEmpty())            
                query = query.Where(x => x.Email.Contains(cond.EmailAdd));
            
            if (!cond.FirstName.IsEmpty())
                query = query.Where(x => x.FirstName.Contains(cond.FirstName));

            if (!cond.Code.IsEmpty())
                query = query.Where(x => x.Code.Contains(cond.Code));

            if (cond.RegDateFrom !=null  && cond.RegDateTo !=null)
            {               
                query = query.Where(x => x.CreateDate >= cond.RegDateFrom && x.CreateDate <= cond.RegDateTo);
            }
            #endregion

            result.TotalRecord = query.Count();

            result.Data = query.MapToList<Member,MemberDto>();
            
            return result;
        }

        public SystemResult Register(RegisterMember member)
        { 
            var result = new SystemResult() ;

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
            var result = new SystemResult() { Succeeded =false };
            
            var member  = await baseRepository.GetModelByIdAsync<Member>(Guid.Parse(currentUser.UserId));
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

        public async Task<PageData<FavoriteMchView>> MyFavMerchant(FavoriteCond cond)
        {
            var result = new PageData<FavoriteMchView>();
            cond.FavoriteType = FavoriteType.Merchant;
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);
            if (cacheData == null || (cacheData.MchList?.Any() ?? false))
            { 
                //重新读取喜欢商家数据
            }

            return result;
        }

        public async Task<PageData<FavoriteProductView>> MyFavProduct(FavoriteCond cond)
        {
            var result = new PageData<FavoriteProductView>();
            cond.FavoriteType = FavoriteType.Product;
            string key = CacheKey.Favorite.ToString();
            string field = CurrentUser.UserId;
            var cacheData = await RedisHelper.HGetAsync<Favorite>(key, field);
            if (cacheData == null || (cacheData.ProductList?.Any() ?? false))
            {
                //重新读取喜欢产品数据
            }

            return result;
        }
    }
}
