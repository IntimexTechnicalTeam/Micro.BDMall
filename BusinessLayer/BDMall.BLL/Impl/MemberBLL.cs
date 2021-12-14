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

            if (!cond.SortName.IsEmpty() && !cond.SortOrder.IsEmpty())
            {
                var sortBy = (SortType)Enum.Parse(typeof(SortType), cond.SortOrder.ToUpper());
                query = query.AsQueryable().SortBy(cond.SortName, sortBy);
            }
            else
                query = query.Skip(cond.Offset).Take(cond.PageSize);

            result.Data = query.MapToList<Member,MemberDto>();
            
            return result;
        }

        public SystemResult CreateMember(RegisterMember member)
        { 
            var result = new SystemResult() ;

            member.Validate();

            productRepository.UpdateProduct();

            //var mc = new MemberCreate<MemberDto>() { Param = member };             
            //this.Mediator.Publish(mc);

            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> ChangeLang(CurrentUser currentUser, Language Lang)
        {
            var result = new SystemResult() { Succeeded =false };
            
            var member  = await baseRepository.GetModelByIdAsync<Member>(currentUser.UserId);
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

            var member = await baseRepository.GetModelByIdAsync<Member>(currentUser.UserId);
            member.CurrencyCode = CurrencyCode;

            if (currentUser.IsLogin)
                await baseRepository.UpdateAsync(member);

            var newToken = jwtToken.RefreshToken(CurrentUser.Token, null, CurrencyCode);

            result.ReturnValue = newToken;
            result.Succeeded = true;
            return result;
        }
    }
}
