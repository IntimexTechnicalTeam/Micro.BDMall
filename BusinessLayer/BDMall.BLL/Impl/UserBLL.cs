using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class UserBLL : BaseBLL, IUserBLL
    {
        IUserRoleRepository userRoleRepository;

        public UserBLL(IServiceProvider services) : base(services)
        {
            userRoleRepository = Services.Resolve<IUserRoleRepository>();
        }

        public async Task<SystemResult> ChangeLang(CurrentUser currentUser, Language Lang)
        {
            var result = new SystemResult() { Succeeded = false };

            var user = await baseRepository.GetModelByIdAsync<User>(currentUser.UserId);
            user.Language = Lang;

            if (currentUser.IsLogin)
                await baseRepository.UpdateAsync(user);

            var newToken = jwtToken.RefreshToken(CurrentUser.Token, Lang, "");

            result.ReturnValue = newToken;
            result.Succeeded = true;
            return result;
        }

        public bool CheckMerchantAccountExist(Guid merchantId)
        {
            var result = userRoleRepository.CheckMerchantAccountExist(merchantId);
            return result;
        }
    }
}
