using BDMall.Model;
using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    //public delegate void CreatedMember(Member member);
    //public delegate void PasswordChanged(MemberInfo member);

    public interface IMemberBLL : IDependency
    {

        PageData<MemberDto> SearchMember(MbrSearchCond cond);

        SystemResult CreateMember(RegisterMember member);
    }
}
