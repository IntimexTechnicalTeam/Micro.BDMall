using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web.Framework;

namespace Web.Mvc
{
    public static class JwtHelper
    {
        public static bool ValidateToken(string token, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            TokenValidationParameters validationParameters = new TokenValidationParameters();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Globals.Configuration["Jwt:SecretKey"]));

            //validationParameters.TokenDecryptionKey = securityKey;
            validationParameters.ValidateLifetime = true; //是否验证失效时间
            validationParameters.ClockSkew = TimeSpan.FromSeconds(30);
            validationParameters.ValidateAudience = false; //是否验证Audience
            validationParameters.ValidateIssuer = false; //是否验证Issuer
            validationParameters.ValidIssuer = null; //Issuer，这两项和前面签发jwt的设置一致
            validationParameters.ValidateIssuerSigningKey = true; //是否验证SecurityKey
            validationParameters.IssuerSigningKey = securityKey; //拿到SecurityKey

            SecurityToken validatedToken;

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (SecurityTokenExpiredException ex)
            {
                //过期了
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
