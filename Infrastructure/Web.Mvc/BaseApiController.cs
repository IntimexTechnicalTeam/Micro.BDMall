using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Jwt;

namespace Web.Mvc
{
    public abstract class BaseApiController : Controller
    {
        public IComponentContext Services;

        /// <summary>
        /// 默认注入AutoFac的IComponentContext
        /// </summary>
        /// <param name="service"></param>
        public BaseApiController(IComponentContext services)
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
                    this._configuration = this.Services.Resolve(typeof(IConfiguration)) as IConfiguration;
                }

                return this._configuration;
            }
        }

        ILogger _logger = null;
        public ILogger Logger
        {
            get
            {
                if (this._logger == null)
                {
                    //ILoggerFactory loggerFactory = this.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;

                    ILoggerFactory loggerFactory = Services.Resolve<ILoggerFactory>();
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

        IMediator _mediator;
        public IMediator Mediator
        {
            get
            {
                if (_mediator == null)
                {
                    _mediator = Services.Resolve<IMediator>();
                }
                return this._mediator;
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

        public string GetClientIP
        {
            get {

                return CurrentContext.HttpContext.GetClientIP();
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

        //public string AutoGenerateNumber(string perfix = "BD")
        //{
        //    return $"{perfix}{IdGenerator.NewId}";
        //}
    }
}
