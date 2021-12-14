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
    public class UserBLL : BaseBLL, IUserBLL
    {
        public UserBLL(IServiceProvider services) : base(services)
        {
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

    }
}
