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
    public class LoginBLL : BaseBLL, ILoginBLL
    {
        IUserRoleRepository userRoleRepository;
        public LoginBLL(IServiceProvider services) : base(services)
        {
             userRoleRepository = Services.Resolve<IUserRoleRepository>();
        }

        public async Task<SystemResult> Login(LoginInput input) {

            var result = new SystemResult() ;
            var user = await baseRepository.GetModelAsync<Member>(x => x.Id == Guid.Parse("E3F18C3B-5D1E-4ACC-B716-B128693BD107"));
            result.ReturnValue = AutoMapperExt.MapTo<MemberDto>(user);
            result.Succeeded = true;          
            return result;
        }

        public async Task<SystemResult> AdminLogin(UserDto user)
        { 
            var result = new SystemResult() ;
          
            var roles = this.userRoleRepository.GetUserRoles(user.Id);
            user.Roles = AutoMapperExt.MapTo<List<RoleDto>>(roles);

            user.LoginType = user.MerchantId != Guid.Empty ? LoginType.Merchant : LoginType.Admin;

            foreach (var item in user.Roles)
            {
                if (item.IsSystem && item.Name == "SuperAdmin")
                {
                    var permissionList = await baseRepository.GetListAsync<Permission>(x => x.IsActive && !x.IsDeleted);
                    item.PermissionList = AutoMapperExt.MapTo<List<PermissionDto>>(permissionList);
                }
            }

            result.ReturnValue = user;
            result.Succeeded = true;

            return result;
        }

        public async Task<UserDto> CheckAdminLogin(LoginInput input)
        {
            var result = new SystemResult();
            string pwd = ToolUtil.Md5Encrypt(input.Password);

            var accounts = baseRepository.GetModel<User>(d => d.IsActive && !d.IsDeleted && (d.Account == input.Account || d.Email == input.Account) && d.Password == pwd);
            if (accounts == null)
                throw new ServiceException("错误的账号或密码");

            var user = AutoMapperExt.MapTo<UserDto>(accounts);

            if (user.MerchantId != Guid.Empty)
            {
                user.LoginType = LoginType.Merchant;
            }
            else
            {
                user.LoginType = LoginType.Admin;
            }

            return user;
        }

    }
}
