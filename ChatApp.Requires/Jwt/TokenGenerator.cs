using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatApp.Requires.Jwt
{
    public class TokenGenerator
    {
        IConfiguration _configuration;
        JsonWebKey _jwt;
        JsonWebKey _jwtRefreshToken;
        string _issuer;
        string _audience;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _jwt = new JsonWebKey(configuration["jwt-key"]);
            _jwtRefreshToken = new JsonWebKey(configuration["jwt-refresh-token"]);
        }

        public string Generate<T>(UserSession<T> session, Guid sessionId, long expiredTimeSecond, bool isRefreshToken = false)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                        new Claim("UserId", session.UserID.ToString()),
                        new Claim("Data", JsonSerializer.Serialize(session.Data)),
                        new Claim("SessionId", sessionId.ToString()),
                }),
                Issuer = _issuer,
                Audience = _audience,
                Expires = DateTime.Now.AddSeconds(expiredTimeSecond),
                SigningCredentials = new SigningCredentials(!isRefreshToken ? _jwt : _jwtRefreshToken, "RS256"),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        public UserSession<T> Validate<T>(string token, bool isRefreshToken = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = !isRefreshToken ? _jwt : _jwtRefreshToken,
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                // Extract claims and map them to UserSession<T> object
                var userSession = new UserSession<T>
                {
                    UserID = long.Parse(principal.FindFirst("UserId").Value),
                    Data = JsonSerializer.Deserialize<T>(principal.FindFirst("Data").Value),
                    SessionID = Guid.Parse(principal.FindFirst("SessionId").Value),
                };

                return userSession;
            }
            catch (Exception)
            {
                // Token validation failed
                return null;
            }
        }
    }
}
