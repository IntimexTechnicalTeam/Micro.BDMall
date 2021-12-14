using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Mvc
{
    /// <summary>
    /// jwt 授权
    /// </summary>
    public class JwtAuthenticationMiddleware
    {
        RequestDelegate _next;
        ILogger<JwtAuthenticationMiddleware> _logger;
        IConfiguration _config;

        public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger, IConfiguration config)
        {
            this._next = next;
            this._logger = logger;
            this._config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            string bearerToken = context.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(bearerToken))
            {
                await this._next(context);
                return;
            }

            string[] arr = bearerToken.Split(' ');

            string token = null;
            if (arr.Length > 1)
            {
                token = arr[1];
            }

            if (string.IsNullOrEmpty(token))
            {
                await this._next(context);
                return;
            }

            ClaimsPrincipal claimsPrincipal;
            bool validateResult = JwtHelper.ValidateToken(token, out claimsPrincipal);

            if (!validateResult)
            {
                await this._next(context);
                return;
            }

            context.User = claimsPrincipal;
            await this._next(context);
        }
    }
}
