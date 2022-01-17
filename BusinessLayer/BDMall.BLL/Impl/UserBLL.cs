using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
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
            if (checkAccount)
            {
                throw new BLException(Resources.Message.AccountExist + ":" + model.Account);
            }
            checkAccount = baseRepository.Any<User>(d => d.Email == model.Email && !d.IsDeleted);
            if (checkAccount)
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

        public PageData<UserDto> Search(UserCondition condition)
        {
            var query = from u in UnitOfWork.DataContext.Users
                        join m in UnitOfWork.DataContext.Merchants on u.MerchantId equals m.Id into ums
                        from um in ums.DefaultIfEmpty()
                        join t in UnitOfWork.DataContext.Translations on um.NameTransId equals t.TransId into uts
                        from ut in uts.DefaultIfEmpty()
                        where (ut == null || ut.Lang == CurrentUser.Lang)
                        select new UserDto
                        {
                            Id = u.Id,                           
                            MerchantId = u.MerchantId,
                            Name = u.Name,
                            // Mobile = u.Mobile,                            
                            MerchantName = ut.Value,
                            Account = u.Account,
                            Email = u.Email,
                            DateTimeFormat = u.DateTimeFormat,
                            //FirstName = u.FirstName,
                            //LastName = u.LastName,
                            LastLogin = u.LastLogin,
                            IsActive = u.IsActive,
                            IsDeleted = u.IsDeleted,
                            CreateDate = u.CreateDate,
                            UpdateDate = u.UpdateDate.Value
                        };


            #region query condition
            if (condition.IsActive.HasValue)
            {
                query = query.Where(d => d.IsActive == condition.IsActive.Value);
            }
            if (condition.IsDeleted.HasValue)
            {
                query = query.Where(d => d.IsDeleted == condition.IsDeleted.Value);
            }

            if (!string.IsNullOrEmpty(condition.Email))
            {
                query = query.Where(d => d.Email.Contains(condition.Email));
            }

            if (!string.IsNullOrEmpty(condition.UserName))
            {
                query = query.Where(d => d.Account.Contains(condition.UserName));
            }
         
            #endregion
            var result = new PageData<UserDto>(condition.PageInfo);
            result.TotalRecord = query.Select(d => d.Id).Count();

            var list = query.OrderBy(d => d.Account).Skip(condition.PageInfo.Offset).Take(condition.PageInfo.PageSize).ToList();

            foreach (var item in list)
            {               
                var roles = userRoleRepository.GetUserRoles(item.Id);
                item.Roles = AutoMapperExt.MapTo<List<RoleDto>>(roles);

                foreach (var role in item.Roles)
                {
                    role.DisplayName = translationRepository.GetDescForLang(role.FullNameTransId, CurrentUser.Lang) ?? "";
                }
            }
            result.Data = list;
            return result;
        }

        public UserDto GetById(Guid userId)
        {
            var user = new UserDto();
            var dbuser = baseRepository.GetModelById<User>(userId);
            if (dbuser != null)
            {
                user = AutoMapperExt.MapTo<UserDto>(dbuser);
                var roles = userRoleRepository.GetUserRoles(userId);
                user.Roles = AutoMapperExt.MapTo<List<RoleDto>>(roles);
            }
            user.Password = "";
            return user;
        }

        public SystemResult Remove(Guid Id)
        {
            var result = new SystemResult();
            var user =baseRepository.GetModelById<User>(Id);

            if (user == null)
            {
                result.Message = BDMall.Resources.Message.UserNotExist;
                return result;
            }
            user.IsDeleted = true;
            baseRepository.Update(user);
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.DeleteSucceeded;

            return result;
        }

        public SystemResult PhysicalDelete(UserDto model)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;

            var entity = baseRepository.GetModelById<User>(model.Id);
            if (entity == null)
            {
                result.Message = Resources.Message.UserNotExist;
                return result;
            }
                  
            var userRoles = baseRepository.GetList<UserRole>(d => d.UserId == entity.Id && d.IsActive && !d.IsDeleted).ToList();

            baseRepository.Delete(userRoles);
            baseRepository.Delete(entity);

            UnitOfWork.Submit();

            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.DeleteSucceeded;
            return result;

        }

        public SystemResult ResetPassword(Guid id)
        {
            SystemResult result = new SystemResult();
            var user = GetById(id);
            if (user == null)
            {
                result.Message = BDMall.Resources.Message.UserNotExist;
                return result;
            }

            var password = "888888";
            user.Password = ToolUtil.Md5Encrypt(password);
            baseRepository.Update(user);

            //user.Password = password;
            //MessageBLL.SendTempPwdToUser(user);   //发送邮件

            result.Message = BDMall.Resources.Message.SaveSuccess;
            result.Succeeded = true;

            return result;
        }

        public SystemResult Update(UserDto model)
        {
            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;
           
            #region check data
            if (string.IsNullOrEmpty(model.Account)) throw new BLException("Account is required");        
            if (string.IsNullOrEmpty(model.Email)) throw new BLException("Email is required");
            
            var entity = baseRepository.GetModelById<User>(model.Id);
            if (entity == null || entity.IsDeleted) throw new BLException(BDMall.Resources.Message.UserNotExist);           
            
            if (model.Account != entity.Account)
            {
                var existSameName = baseRepository.Any<User>(d => d.Id != model.Id && d.Account == model.Account && !d.IsDeleted);
                if (existSameName) throw new BLException(BDMall.Resources.Message.AccountExist);
            }

            if (model.Email != entity.Email)
            {
                var existSameEmail = baseRepository.Any<User>(d => d.Id != model.Id && d.Email == model.Email && !d.IsDeleted);
                if (existSameEmail) throw new BLException(BDMall.Resources.Message.EmailExist);
            }
            #endregion

            entity.Account = model.Account;
            entity.Name = model.Name ?? "";

            if (entity.MerchantId != model.MerchantId)
            {             
                var oldMerchantUser = baseRepository.Any<User>(p => p.MerchantId == entity.MerchantId && p.Id != entity.Id);
                if (!oldMerchantUser) throw new BLException(BDMall.Resources.Message.OldMerchantMbrEmpty);              
            }

            entity.Email = model.Email;
            entity.IsActive = model.IsActive;
            entity.Language = model.Language;
            entity.MerchantId = model.MerchantId;
            //entity.IsDeleted = model.IsDeleted; 更新用戶資料，不能在此更新刪除狀態 
            if (!string.IsNullOrEmpty(model.Password))
            {
                entity.Password = ToolUtil.Md5Encrypt(model.Password);
            }
            baseRepository.Update(entity);

            #region user role relationship
            if (model.Id.ToString() != CurrentUser.UserId)
            {
                if (model.Roles?.Any() ?? false)
                {
                    var oldUserRole = baseRepository.GetList<UserRole>(d => d.UserId == model.Id && d.IsActive && !d.IsDeleted).ToList();
                    baseRepository.Delete(oldUserRole);

                    foreach (var r in model.Roles)
                    {
                        UserRole userRole = new UserRole();
                        userRole.Id = Guid.NewGuid();
                        userRole.UserId = model.Id;
                        userRole.RoleId = r.Id;
                        baseRepository.Insert(userRole);
                    }
                }
            }
            #endregion

            UnitOfWork.Submit();
            result.Succeeded = true;
            result.Message = BDMall.Resources.Message.SaveSuccess;
             
            return result;
        }
    }
}
