using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IUserBLL:IDependency
    {

        Task<SystemResult> ChangeLang(CurrentUser currentUser, Language Lang);

        bool CheckMerchantAccountExist(Guid merchantId);

        SystemResult CreateAccountForMerchant(string Ids);

        SystemResult Save(UserDto model);

        UserDto GetUserInfoById(string UserId);

        PageData<UserDto> Search(UserCondition condition);

        UserDto GetById(Guid userId);

        SystemResult Remove(Guid Id);

        SystemResult PhysicalDelete(UserDto model);

        SystemResult ResetPassword(Guid id);

        SystemResult Update(UserDto model);
    }
}
