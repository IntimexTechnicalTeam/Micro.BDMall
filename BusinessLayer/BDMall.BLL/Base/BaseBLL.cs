
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using Intimex.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Framework;
using Web.Jwt;
using Web.MQ;

namespace BDMall.BLL
{
    /// <summary>
    /// 业务逻辑层继承此类
    /// </summary>
    public class BaseBLL
    {
        public IServiceProvider Services;
 
        //public ILoginBLL loginBLL;
        public BaseBLL(IServiceProvider services)
        {
            this.Services = services;
        }
        IConfiguration _configuration;
        public IConfiguration Configuration
        {
            get
            {
                if (this._configuration == null)
                {
                    this._configuration = this.Services.GetService(typeof(IConfiguration)) as IConfiguration;
                }

                return this._configuration;
            }
        }

        IMediator _mediator;
        public IMediator mediator
        {
            get
            {
                if (this._mediator == null)
                {
                    this._mediator = this.Services.GetService(typeof(IMediator)) as IMediator;
                }

                return this._mediator;
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

        IUnitOfWork _unitOfWork;
        public IUnitOfWork UnitOfWork {

            get
            {
                if (this._unitOfWork == null)
                    this._unitOfWork = this.Services.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
                return this._unitOfWork;
            }
        }

        IRabbitMQService _rabbitMQService;
        public IRabbitMQService rabbitMQService
        {
            get
            {
                if (this._rabbitMQService == null)
                {
                    this._rabbitMQService = this.Services.GetService(typeof(IRabbitMQService)) as IRabbitMQService;
                }

                return this._rabbitMQService;
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
        public ILoginBLL _loginBLL;
        public ILoginBLL loginBLL
        {
            get
            {
                if (_loginBLL == null)
                {
                    _loginBLL = Services.Resolve<ILoginBLL>();
                }
                return this._loginBLL;
            }
        }

        public ICurrencyBLL _currencyBLL;
        public ICurrencyBLL currencyBLL
        {
            get
            {
                if (_currencyBLL == null)
                {
                    _currencyBLL = Services.Resolve<ICurrencyBLL>();
                }
                return this._currencyBLL;
            }
        }

        IHostEnvironment _hostEnvironment;
        public IHostEnvironment hostEnvironment
        {
            get
            {
                if (_hostEnvironment == null)
                {
                    _hostEnvironment = Services.Resolve<IHostEnvironment>();
                }
                return this._hostEnvironment;
            }
        }

        CurrentUser _currentUser;
        public CurrentUser CurrentUser
        {
            get
            {
                string token = CurrentContext?.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length).Trim() ?? "";
                if (token.IsEmpty()) token = CurrentContext?.HttpContext.Request?.Cookies["access_token"]?.ToString() ?? "";

                _currentUser = jwtToken.BuildUser(token, _currentUser, x => (loginBLL.AdminLogin(new UserDto { Id = Guid.Parse(_currentUser.UserId) })).Result);

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

        /// <summary>
        /// 判斷是否與基準貨幣一致
        /// </summary>
        /// <param name="checkCurrency">檢查的貨幣資料</param>
        public bool IsMatchBaseCurrency(string CurrencyCode)
        {
            bool result = false;
            var checkCurrency = currencyBLL.GetSimpleCurrency(CurrencyCode);
            if (checkCurrency != null)
            {
                var baseCurrency = currencyBLL.GetDefaultCurrency();
                if (baseCurrency != null && !string.IsNullOrEmpty(baseCurrency.Code))
                {
                    if (baseCurrency.Code == checkCurrency.Code)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 將金額使用基準貨幣兌換率換算成當前貨幣金額
        /// </summary>
        /// <param name="baseMoney">基準貨幣類型金額</param>
        /// <returns>換算後的當前貨幣金額</returns>
        public decimal MoneyConversion(decimal baseMoney)
        {
            var myCurrency = currencyBLL.GetSimpleCurrency(CurrentUser.CurrencyCode);
            decimal curMoney = baseMoney * myCurrency.ExchangeRate;
            curMoney = Math.Round(curMoney, 2);
            return curMoney;
        }

        public string AutoGenerateNumber(string perfix="BD")
        {
            return $"{perfix}{IdGenerator.NewId}";
        }
    }

    public class ServiceBase<TDal> : BaseBLL
    {
        TDal _dal;

        public ServiceBase(IServiceProvider services) : base(services)
        {

        }

        public new TDal Dal
        {
            get
            {
                if (this._dal == null)
                    this._dal = (TDal)this.Services.GetService(typeof(TDal));

                return this._dal;
            }
        }
    }
}
