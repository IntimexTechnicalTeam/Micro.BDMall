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
    }
}
