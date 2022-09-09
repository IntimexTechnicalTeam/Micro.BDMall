using BDMall.Domain;
using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace Web.Jwt
{
    public interface IJwtToken:IDependency
    {       
        /// <summary>
        /// 针对TempUser生成的一个DefaultToken
        /// </summary>
        /// <returns></returns>
        string CreateDefautToken();

        /// <summary>
        /// 根据Token生成CurrentUser
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <returns></returns>
        CurrentUser CreateCurrentUser(string encodeJwt);

        CurrentUser BuildUser(string token, CurrentUser _currentUser, Func<string, SystemResult> func=null);

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claimList"></param>
        /// <returns></returns>
        string CreateToken<T>(T user) where T : class;

        string CreateToken(List<Claim> claimList);

        /// <summary>
        /// 重新刷新Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="Lang"></param>
        /// <param name="CurrencyCode"></param>
        /// <returns></returns>
        string RefreshToken(string token, Language? Lang = null, string CurrencyCode = "");

        /// <summary>
        ///  解析token中的payload信息,payload信息来源于tokeninfo类
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <returns></returns>
        Dictionary<string, string> DecodeJwt(string encodeJwt);

        bool Validate(string encodeJwt, Func<Dictionary<string, string>, bool> validatePayLoad = null);

        TokenType Validate(string encodeJwt);

        bool ValidatePayLoad(string encodeJwt, out Dictionary<string, string> outpayLoad, Func<Dictionary<string, string>, bool> validatePayLoad = null);

        TokenType ValidatePlus(string encodeJwt, Func<Dictionary<string, string>, bool> validatePayLoad, Action<Dictionary<string, string>> action);

    }
}
