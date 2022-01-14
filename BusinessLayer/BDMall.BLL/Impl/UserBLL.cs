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
        ITranslationRepository translationRepository;
        public UserBLL(IServiceProvider services) : base(services)
        {
            userRoleRepository = Services.Resolve<IUserRoleRepository>();
            translationRepository = Services.Resolve<ITranslationRepository>();
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

        /// <summary>
        /// 创建商家账号
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        /// <exception cref="BLException"></exception>
        public SystemResult CreateAccountForMerchant(string Ids)
        {
            var result = new SystemResult();
            if (Ids.IsEmpty())
                throw new BLException();

            var userList = new List<User>();
            var userRoleList = new List<UserRole>();
            var merchantIds = Ids.TrimEnd(',').Split(',').Select(s=>Guid.Parse(s)).ToList();
            foreach (var item in merchantIds)
            {
                //检查是否有账号了
                if (CheckMerchantAccountExist(item))
                {
                    result.Message += BDMall.Resources.Message.MerchantAccountCreated;
                    break;
                }

                var flag  = baseRepository.Any<Merchant>(x=>x.Id == item);
                if (!flag)             
                    throw new BLException("no merchant found.");

                var accountData = GenAccountForMerchant(item);
                userRoleList.AddRange(accountData.Item2);
                userList.Add(accountData.Item1);
                result.Succeeded = true;
            }

            if (result.Succeeded)
            {
                using var tran = baseRepository.CreateTransation();
                baseRepository.Insert(userList);
                baseRepository.Insert(userRoleList);
                tran.Commit();

                result.Succeeded = true;
                result.Message = Resources.Message.CreateAccountSucess;
            }

            return result;
        }

        public Tuple<User,List<UserRole>> GenAccountForMerchant(Guid merchantId)
        {
            SystemResult result = new SystemResult();
            Merchant merchant = baseRepository.GetModelById<Merchant>(merchantId);

            var user = new User();
            user.Id = Guid.NewGuid();
            user.Account = merchant.ContactEmail;
            user.Email = merchant.ContactEmail;
            user.Name = translationRepository.GetTranslation(merchant.ContactTransId, merchant.Language)?.Value;
            user.MerchantId = merchantId;
            user.Language = merchant.Language;
            user.CreateDate = DateTime.Now;
            user.CreateBy = Guid.Parse(CurrentUser.UserId);
            user.LastLogin = new DateTime(1970, 1, 1, 0, 0, 0);
            //string password = GetPassword();
            //user.Password = HashUtil.HashPwd(password);
            user.Password = ToolUtil.Md5Encrypt("888888");
            var userRoles = new List<UserRole>();
            if (merchant.IsExternal)
            {
                if (merchant.MerchantType == MerchantType.GS1)
                {
                    userRoles.Add(new UserRole()
                    {
                        Id =Guid.NewGuid(),
                        RoleId = new Guid(StoreConst.ExternalGS1MerchantAdminRoleId),
                        CreateBy = Guid.Parse(CurrentUser.UserId),
                        CreateDate = DateTime.Now,
                        UserId = user.Id
                    });
                }
                else
                {
                    userRoles.Add(new UserRole()
                    {
                        Id = Guid.NewGuid(),
                        RoleId = new Guid(StoreConst.ExternalMerchantAdminRoleId),
                        CreateBy = Guid.Parse(CurrentUser.UserId),
                        CreateDate = DateTime.Now,
                        UserId = user.Id
                    });
                }
            }
            else
            {
                if (merchant.MerchantType == MerchantType.GS1)
                {
                    userRoles.Add(new UserRole()
                    {
                        Id = Guid.NewGuid(),
                        RoleId = new Guid(StoreConst.InternalGS1MerchantAdminRoleId),
                        CreateBy = Guid.Parse(CurrentUser.UserId),
                        CreateDate = DateTime.Now,
                        UserId = user.Id
                    }); 
                }
                else
                {
                    userRoles.Add(new UserRole()
                    {
                        Id = Guid.NewGuid(),
                        RoleId = new Guid(StoreConst.InternalMerchantAdminRoleId),
                        CreateBy = Guid.Parse(CurrentUser.UserId),
                        CreateDate = DateTime.Now,
                        UserId = user.Id
                    });
                }
            }

            return new Tuple<User, List<UserRole>>(user,userRoles);
        }


        public SystemResult Save(UserDto model)
        {
            SystemResult result = new SystemResult();
            
            #region checking
            var checkAccount = baseRepository.Any<User>(d => d.Account == model.Account && !d.IsDeleted);
            if (!checkAccount)
            {
                throw new BLException(Resources.Message.AccountExist + ":" + model.Account);
            }
            checkAccount = baseRepository.Any<User>(d => d.Email == model.Email && !d.IsDeleted);
            if (!checkAccount)
            {
                throw new BLException(Resources.Message.EmailExist + ":" + model.Email);
            }
            #endregion
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                model.Name = model.Account;
            }
            //string password = model.Password;
            //if (string.IsNullOrEmpty(model.Password))
            //{
            //    password = GetPassword();
            //    model.Password = HashUtil.HashPwd(password);
            //}
            model.Password = ToolUtil.Md5Encrypt("888888");
            var userRoles = model.Roles.Select(s => new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = model.Id,
                RoleId = s.Id,
                CreateDate = DateTime.Now,
                CreateBy = Guid.Parse(CurrentUser.UserId)
            }).ToList();

            var user = AutoMapperExt.MapTo<User>(model);
            user.CreateBy = Guid.Parse(CurrentUser.UserId);
            user.CreateDate = DateTime.Now; 

            using var tran = baseRepository.CreateTransation();
            baseRepository.Insert(user);
            baseRepository.Insert(userRoles);
            tran.Commit();

            result.Succeeded = true;
            result.Message = Resources.Message.CreateAccountSucess;
            return result;
        }

        public UserDto GetUserInfoById(string UserId)
        { 
            var dbModel = baseRepository.GetModel<User>(x=>x.Id == Guid.Parse(UserId));
            var user = AutoMapperExt.MapTo<UserDto>(dbModel);
            return user;
        }
    }
}
