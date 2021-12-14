using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface ILoginBLL:IDependency
    {

        Task<SystemResult> Login(LoginInput input);

        Task<SystemResult> AdminLogin(UserDto user);

        Task<UserDto> CheckAdminLogin(LoginInput input);
    }
}
