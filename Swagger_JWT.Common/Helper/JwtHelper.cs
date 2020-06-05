using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Swagger_JWT.Common.Helper
{
    public class JwtHelper
    {

        private readonly IConfiguration Configuration;

        public JwtHelper(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        /// <summary>
        /// 頒发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public  string IssueJwt(TokenModelJwt tokenModel)
        {
            string iss = Configuration.GetValue<string>("JwtSettings:Issuer");
            string aud = Configuration.GetValue<string>("JwtSettings:Audience");
            string secret = Configuration.GetValue<string>("JwtSettings:SignKey");

            //var claims = new Claim[] //old
            var claims = new List<Claim>
                {
                 /*
                 * 特別重要：
                   1、這里將用戶的部分信息，比如 uid 存到了Claim 中，如果你想知道如何在其他地方將這個 uid從 Token 中取出來，請看下邊的SerializeJwt() 方法，或者在整個解決方案，搜索這個方法，看哪里使用了！
                 */

                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //這個就是過期時間，目前是過期1000秒，可自定義，注意JWT有自己的緩沖過期時間
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(1000).ToString()),
                 new Claim(JwtRegisteredClaimNames.Sub, tokenModel.Name),// User.Identity.Name
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                 new Claim(JwtRegisteredClaimNames.Aud,aud),

            //new Claim(ClaimTypes.Role,tokenModel.Role),//為了解決一個用戶多個角色(比如：Admin,System)，用下邊的方法
        };

            // 可以將一個用戶的多個角色全部賦予；
            
            claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));



            //秘鑰 (SymmetricSecurityKey 對安全性的要求，密鑰的長度太短會報出異常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModelJwt
            {
                Uid = (jwtToken.Id).ObjToInt(),
                Role = role != null ? role.ObjToString() : "",
            };
            return tm;
        }
    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 職能
        /// </summary>
        public string Work { get; set; }
        /// <summary>
        /// 編號
        /// </summary>
        public string Name { get; set; }

    }
}

