using BDMall.Domain;
using BDMall.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace Web.Jwt
{
    public class JwtToken:IJwtToken
    {
        IServiceProvider service;
        IConfiguration configuration;
         
        public JwtToken(IServiceProvider _service)
        {
            this.service = _service;
            configuration = service.Resolve<IConfiguration>();
        }

        public string CreateToken<T>(T user) where T : class
        {
            //携带的负载部分，类似一个键值对
            List<Claim> claims = new List<Claim>();
            //这里我们用反射把model数据提供给它
            foreach (var item in user.GetType().GetProperties())
            {
                object obj = item.GetValue(user);
                string value = "";
                if (obj != null)
                    value = obj.ToString();

                claims.Add(new Claim(item.Name, value));
            }
            //创建token
            return CreateToken(claims);
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claimList"></param>
        /// <returns></returns>
        public string CreateToken(List<Claim> claimList)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:SecretKey"]));
            int expireMinute = int.Parse(this.configuration["Jwt:ExpireMinute"]);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    issuer: this.configuration["Jwt:Issuer"],                           //TOKEN发布者
                    audience: this.configuration["Jwt:Audience"],                //Token接受者
                    expires: DateTime.Now.AddDays(expireMinute),
                    notBefore :DateTime.Now,                                            //nbf缩写
                    signingCredentials: creds,
                    claims: claimList
                    );

            var ticket = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return ticket;
        }

        /// <summary>
        /// 重新刷新Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string RefreshToken(string token, Language? Lang = null, string CurrencyCode = "")
        {          
            var tmpUser = CreateCurrentUser(token);
            tmpUser.Lang = Lang != null ? Lang.Value : tmpUser.Lang;
            tmpUser.CurrencyCode = !CurrencyCode.IsEmpty() ? CurrencyCode : tmpUser.CurrencyCode;
            var loginInput = AutoMapperExt.MapTo<TokenInfo>(tmpUser);
            return CreateToken(loginInput);
        }

        /// <summary>
        /// 针对TempUser生成的一个DefaultToken
        /// </summary>
        /// <returns></returns>
        public string CreateDefautToken()
        {
            var tempUser = new TokenInfo
            {
                UserId = Guid.NewGuid().ToString(),
                Account = "AnonymousUser",
                IsLogin = false,
                LoginType = LoginType.TempUser,
                CurrencyCode = "HKD",
                Lang = Language.C,
            };
            string ticket = CreateToken(tempUser);
            return ticket;
        }

        /// <summary>
        /// 根据Token生成CurrentUser
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <returns></returns>
        public CurrentUser CreateCurrentUser(string encodeJwt)
        {
            var currentUser = new CurrentUser();

            if (encodeJwt.IsEmpty()) return currentUser;

            var payload = DecodeJwt(encodeJwt);

            var prop = currentUser.GetType().GetProperties();
            foreach (var item in prop)
            {
                if (payload.Any(x => x.Key == item.Name))
                {
                    if (item.PropertyType == typeof(string))
                    {
                        item.SetValue(currentUser, payload[item.Name]);
                    }
                    else if (item.PropertyType.IsEnum)
                    {
                        if (item.PropertyType.Name == "Language")
                            item.SetValue(currentUser, payload[item.Name].ToEnum<Language>());
                        else if (item.PropertyType.Name == "LoginType")
                            item.SetValue(currentUser, payload[item.Name].ToEnum<LoginType>());
                    }
                    else if (item.PropertyType == typeof(bool))
                    {
                        item.SetValue(currentUser, bool.Parse(payload[item.Name]));
                    }
                }
            }
            return currentUser;
        }

        public CurrentUser BuildUser(string token, CurrentUser _currentUser,Func<string,SystemResult> func=null)
        {
            if (_currentUser == null || token.IsEmpty())
            {
                _currentUser = new CurrentUser();
                //return _currentUser;
            }

            _currentUser = CreateCurrentUser(token);
            //admin,商家和第三方商家
            if (_currentUser.LoginType <= LoginType.Admin)
            {
                //加载用户角色，先从缓存读
                string key = $"{CacheKey.CurrentUser}";
                var cacheUser = RedisHelper.HGet<UserDto>(key, _currentUser.UserId);
                if (!cacheUser?.Roles?.Any() ?? false)
                {
                    if (func != null)
                    {
                        var result = func.Invoke(_currentUser.UserId);
                        var userInfo = result.ReturnValue as UserDto;
                        RedisHelper.HSetAsync($"{CacheKey.CurrentUser}", userInfo.Id.ToString(), userInfo);
                        cacheUser.Roles = userInfo.Roles;
                    }
                }
                _currentUser.Roles = cacheUser?.Roles;
                _currentUser.MerchantId = cacheUser?.MerchantId ?? Guid.Empty;
            }
            return _currentUser;
        }

        /// <summary>
        /// 解析token中的payload信息,payload信息来源于tokeninfo类
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <returns></returns>
        /// <exception cref="ApiServiceException"></exception>
        public Dictionary<string, string> DecodeJwt(string encodeJwt)
        {
            if (encodeJwt.IsEmpty()) throw new ApiServiceException(500, "Token不能为空");

            var jwtArr = encodeJwt.Split('.');
            if (jwtArr.Length < 3) throw new ApiServiceException(500, "解析Token出错");

            //var header = JsonUtil.JsonToObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[0]));
            var payLoad = JsonUtil.JsonToObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[1]));
            return payLoad;
        }

        /// <summary>
        /// 验证身份 验证签名的有效性
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <param name="validatePayLoad">自定义各类验证； 是否包含那种申明，或者申明的值，例如：payLoad["aud"]?.ToString() == "AXJ"; </param>
        public bool Validate(string encodeJwt, Func<Dictionary<string, string>, bool> validatePayLoad = null)
        {
            var success = true;
            var jwtArr = encodeJwt.Split('.');
            if (jwtArr.Length < 3) return false;

            var payLoad = DecodeJwt(encodeJwt);
            //配置文件中取出来的签名秘钥
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(this.configuration["Jwt:SecretKey"]));
            //验证签名是否正确（把用户传递的签名部分取出来和服务器生成的签名匹配即可）
            success = success && string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1])))));
            if (!success)
            {
                return success;//签名不正确直接返回
            }

            //其次验证是否在有效期内（也应该必须）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            success = success && (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));

            //不需要自定义验证不传或者传递null即可
            if (validatePayLoad == null)
                return true;

            //再其次 进行自定义的验证
            success = success && validatePayLoad(payLoad);

            return success;
        }

        public bool ValidatePayLoad(string encodeJwt, out Dictionary<string, string> outpayLoad, Func<Dictionary<string, string>, bool> validatePayLoad = null)
        {
            outpayLoad = null;

            var success = true;
            var jwtArr = encodeJwt.Split('.');
            if (jwtArr.Length < 3) return false;

            var payLoad = DecodeJwt(encodeJwt);

            //配置文件中取出来的签名秘钥
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(this.configuration["Jwt:SecretKey"]));
            //验证签名是否正确（把用户传递的签名部分取出来和服务器生成的签名匹配即可）
            success = success && string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1])))));
            if (!success)
            {
                return success;//签名不正确直接返回
            }

            //其次验证是否在有效期内（也应该必须）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            success = success && (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));

            //不需要自定义验证不传或者传递null即可
            if (validatePayLoad == null)
                return true;

            //再其次 进行自定义的验证
            success = success && validatePayLoad(payLoad);

            outpayLoad = payLoad;

            return success;
        }

        /// <summary>
        /// 验证身份 验证签名的有效性
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <param name="validatePayLoad">自定义各类验证； 是否包含那种申明，或者申明的值，例如：payLoad["aud"]?.ToString() == "AXJ"; </param>
        public TokenType ValidatePlus(string encodeJwt, Func<Dictionary<string, string>, bool> validatePayLoad, Action<Dictionary<string, string>> action)
        {
            var jwtArr = encodeJwt.Split('.');
            if (jwtArr.Length < 3) return TokenType.Fail;
            var payLoad = DecodeJwt(encodeJwt);
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(this.configuration["Jwt:SecretKey"]));
            //验证签名是否正确（把用户传递的签名部分取出来和服务器生成的签名匹配即可）
            if (!string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1]))))))
            {
                return TokenType.Fail;
            }

            //其次验证是否在有效期内（必须验证）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            if (!(now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString())))
            {
                return TokenType.Expired;
            }

            //不需要自定义验证不传或者传递null即可
            if (validatePayLoad == null)
            {
                action(payLoad);
                return TokenType.Ok;
            }
            //再其次 进行自定义的验证
            if (!validatePayLoad(payLoad))
            {
                return TokenType.Fail;
            }
            //可能需要获取jwt摘要里边的数据，封装一下方便使用
            action(payLoad);
            return TokenType.Ok;
        }

        public TokenType Validate(string encodeJwt)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(encodeJwt);
             
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(this.configuration["Jwt:SecretKey"]));
            //验证签名是否正确（把用户传递的签名部分取出来和服务器生成的签名匹配即可）
            if (!string.Equals(token.RawSignature, Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(token.RawHeader, ".", token.RawPayload))))))
            {
                return TokenType.Fail;
            }

            //其次验证是否在有效期内（必须验证）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            if (!(now >= long.Parse(token.Claims.FirstOrDefault(x => x.Type == "nbf").Value) && now < long.Parse(token.Claims.FirstOrDefault(x => x.Type == "exp").Value)))
            {
                return TokenType.Expired;
            }

            return TokenType.Ok;
        }

        private long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }

    public enum TokenType { Ok, Fail, Expired }
}
