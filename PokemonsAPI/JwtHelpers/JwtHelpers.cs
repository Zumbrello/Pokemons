using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PokemonsAPI.Models;
namespace PokemonsAPI.JwtHelpers {
public static class JwtHelpers {
        public static UserTokens GenTokenkey(UserTokens model, JwtSettings jwtSettings) {
            try {
                //Установка настроек токена
                var UserToken = new UserTokens();
                if (model == null) throw new ArgumentException(nameof(model));
                
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                Guid Id = Guid.Empty;
                DateTime expireTime = DateTime.UtcNow.AddMinutes(30);
                DateTime expireTimeRefresh = DateTime.Now.AddDays(31);
                
                //Настройка Access токена
                var AccessToken = new JwtSecurityToken(issuer: jwtSettings.ValidIssuer, 
                    audience: jwtSettings.ValidAudience, 
                    expires: expireTime, 
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
                
                //Настройка Refresh токена
                var RefreshToken = new JwtSecurityToken(issuer: jwtSettings.ValidIssuer, 
                    audience: jwtSettings.ValidAudience, 
                    expires: expireTimeRefresh, 
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

                //Добавление значений в токен
                UserToken.Token = new JwtSecurityTokenHandler().WriteToken(AccessToken);
                UserToken.RefreshToken = new JwtSecurityTokenHandler().WriteToken(RefreshToken);
                UserToken.UserName = model.UserName;
                return UserToken;
            } catch (Exception) {
                throw;
            }
        }
    }
}