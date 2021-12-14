using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
                string token = CurrentContext?.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length).Trim() ?? "";
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
    }
}
