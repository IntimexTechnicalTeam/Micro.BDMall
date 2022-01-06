using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using Intimex.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Web.Framework;
using Web.Jwt;

namespace BDMall.Repository
{
    public class PublicBaseRepository
    {
        public IServiceProvider Services;

        public PublicBaseRepository(IServiceProvider service)
        {
            //Id = Guid.NewGuid();
            this.Services = service;
          
        }

        IUnitOfWork _unitOfWork;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (this._unitOfWork == null)
                    this._unitOfWork = this.Services.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
                return this._unitOfWork;
            }
        }

        IBaseRepository _IBaseRepository;
        public IBaseRepository baseRepository
        {
            get
            {
                if (this._IBaseRepository == null)
                    this._IBaseRepository = this.Services.GetService(typeof(IBaseRepository)) as IBaseRepository;
                return this._IBaseRepository;
            }
        }

        MallDbContext _dbContext;
        public MallDbContext DbContext
        {
            get
            {
                if (this._dbContext == null)
                    this._dbContext = this.Services.GetService(typeof(MallDbContext)) as MallDbContext;
                return this._dbContext;
            }
        }

        IHttpContextAccessor _currentContext;
        public IHttpContextAccessor CurrentContext
        {
            get
            {
                if (_currentContext == null)
                {
                    _currentContext = Services.Resolve<IHttpContextAccessor>();
                }
                return this._currentContext;
            }
        }

        public IJwtToken _jwtToken;
        public IJwtToken jwtToken
        {
            get
            {
                if (_jwtToken == null)
                {
                    _jwtToken = Services.Resolve<IJwtToken>();
                }
                return this._jwtToken;
            }
        }

        CurrentUser _currentUser;
        public CurrentUser CurrentUser
        {
            get
            {
                //注意区分BaseMvcController的写法
                string token = CurrentContext?.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length).Trim() ?? "";

                if (token.IsEmpty()) token = CurrentContext?.HttpContext.Request?.Cookies["access_token"].ToString() ?? "";

                if (_currentUser == null || token.IsEmpty())
                {
                    _currentUser = new CurrentUser();
                    //return _currentUser;
                }

                var payload = jwtToken.DecodeJwt(token);
                _currentUser.Token = token;
                _currentUser.UserId = payload["UserId"];
                _currentUser.CurrencyCode = payload["CurrencyCode"];
                _currentUser.Lang = (Language)Enum.Parse(typeof(Language), payload["Lang"]);
                _currentUser.LoginType = (LoginType)Enum.Parse(typeof(LoginType), payload["LoginType"]);

                //admin,商家和第三方商家
                if (_currentUser.LoginType <= LoginType.Admin)
                {
                    //加载用户角色，先从缓存读
                    string key = $"{CacheKey.CurrentUser}";
                    var cacheUser = RedisHelper.HGet<UserDto>(key, _currentUser.UserId);                  
                    _currentUser.MerchantId = cacheUser?.MerchantId ?? Guid.Empty;
                }

                return _currentUser;
            }
        }

        ILogger _logger;
        public ILogger Logger
        {
            get
            {
                if (this._logger == null)
                {
                    ILoggerFactory loggerFactory = this.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                    ILogger logger = loggerFactory.CreateLogger(this.GetType().FullName);
                    this._logger = logger;
                }

                return this._logger;
            }
        }

        protected virtual void LogException(Exception ex)
        {
            string error = "\r\n 异常类型：" + ex.GetType().FullName + "\r\n 异常源：" + ex.Source + "\r\n 异常位置=" + ex.TargetSite + " \r\n 异常信息=" + ex.Message + " \r\n 异常堆栈：" + ex.StackTrace;

            this.Logger.LogError(error);
        }


        public List<SystemLang> GetSupportLanguage()
        {
            return GetSupportLanguage(CurrentUser?.Lang ?? Language.C);
        }

        /// <summary>
        /// 获取系统开通语言
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public List<SystemLang> GetSupportLanguage(Language rtnlang)
        {

            List<SystemLang> langs = new List<SystemLang>();

            var query = baseRepository.GetList<CodeMaster>().Where(d => d.IsActive && !d.IsDeleted);
            query = query.Where(d => d.Module == CodeMasterModule.Setting.ToString());
            query = query.Where(d => d.Function == CodeMasterFunction.SupportLanguage.ToString());

            var data = query.Where(d => d.Value == "1").ToList();//获取系统开通的语言
            var allLanguages = LangUtil.GetAllLanguages(rtnlang);//获取系统支持的语言

            langs.Add(new SystemLang { Code = Language.E.ToString(), Id = (int)Language.E, Text = Resources.Value.LangEnglish });
            langs.Add(new SystemLang { Code = Language.C.ToString(), Id = (int)Language.C, Text = Resources.Value.LangTraditionalChinese });
            langs.Add(new SystemLang { Code = Language.S.ToString(), Id = (int)Language.S, Text = Resources.Value.LangSimplifiedChinese });
            langs.Add(new SystemLang { Code = Language.J.ToString(), Id = (int)Language.J, Text = Resources.Value.LangJapaness });

            if (data != null && data.Any())
            {
                langs = new List<SystemLang>();
                foreach (var item in allLanguages)
                {
                    if (data.FirstOrDefault(d => d.Key == item.Code) != null)
                    {
                        var name = item.Text;
                        SystemLang lang = new SystemLang(name ?? "", item.Code);
                        lang.Id = (int)LangUtil.GetLang(item.Code);
                        langs.Add(lang);
                    }
                }
            }
            return langs;
        }

        public string AutoGenerateNumber(string perfix = "BD")
        {
            return $"{perfix}{IdGenerator.NewId}";
        }
    }
}
