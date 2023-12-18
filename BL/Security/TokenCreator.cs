using BL.Helper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BL.Security
{
    public interface ITokenCreator
    {
        bool IsAuthorize(string token);
        string CreateTokenReportService();
    }
    public class TokenCreator : ITokenCreator
    {
        public WindowsIdentity WindowsIdentity { get; set; }
        public string Name = "";
        public Exception ex;
        bool IsValid { get; set; }
        public TokenCreator()
        {
        }
        public bool IsAuthorize(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                string authHeader = token.Replace("Bearer ", "").Replace(" ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                IsValid = ValidateToken(authHeader);
                bool Claim = !string.IsNullOrEmpty(tokenS.Claims.First(x => x.Type == "iss").Value) &&
                       !string.IsNullOrEmpty(tokenS.Claims.First(x => x.Type == "sub").Value) &&
                       !string.IsNullOrEmpty(tokenS.Claims.First(x => x.Type == "exp").Value);//Проверяем чтобы были все клеймы
                var Lifetime = Convert.ToInt32(tokenS.Claims.First(x => x.Type == "exp").Value) - new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();// преобразуем время жизни токена в секунды затем проверим чтобы было не больше 900 секунд
                if (IsValid && Claim && Lifetime <= 900)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex_)
            {
                return false;
            }

        }
        public string CreateTokenReportService()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.ReportServiceToken).GetString());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("iss", "Service Name"),
                    new Claim("sub", "Auth"),
					/*new Claim("exp",dto.ToUnixTimeSeconds().ToString())*/
				}),
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key)
                , SecurityAlgorithms.HmacSha256Signature)
            };
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                                          //ValidIssuer = "Sample",
                                          //ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.ReportServiceToken).GetString())) // Установить секретный ключ
            };
        }
    }
}
