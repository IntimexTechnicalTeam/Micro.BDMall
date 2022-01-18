using BDMall.Model;
using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;
using BDMall.Enums;

namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IMemberBLL : IDependency
    {

        PageData<MemberDto> SearchMember(MbrSearchCond cond);

        SystemResult Register(RegisterMember member);

        Task<SystemResult> ChangeLang(CurrentUser currentUser, Language Lang);

        Task<SystemResult> ChangeCurrencyCode(CurrentUser currentUser, string CurrencyCode);

        RegSummary GetRegSummary();
    }
}
