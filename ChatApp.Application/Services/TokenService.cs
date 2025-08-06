using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using ChatApp.Infrastructure.Configurations;
using Microsoft.IdentityModel.Tokens;


namespace ChatApp.Application.Services
{
    public class TokenService : ITokenService
    {
        JwtConfig _jwtConfig;
        SigningCredentials _jwtSc;
        SigningCredentials _jwtRefreshSc;
        JwtSecurityTokenHandler _tokenHandler;

        public TokenService(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
            var key = new JsonWebKey(_jwtConfig.JwtKey);
            var refreshKey = new JsonWebKey(_jwtConfig.JwtRefreshKey);
            _jwtSc = new SigningCredentials(key, "RS256");
            _jwtRefreshSc = new SigningCredentials(refreshKey, "RS256");
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public AuthToken GenerateToken<T>(UserSession<T> session)
        {
            var issuer = _jwtConfig.Issuer;
            var audience = _jwtConfig.Audience;

            var userAccessToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("UserId", session.UserID.ToString()),
                    new Claim("Data", JsonSerializer.Serialize(session.Data)),
                    new Claim("SessionId", session.SessionID.ToString()),
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.Now.AddSeconds(_jwtConfig.ExpiredTimeSeconds),
                SigningCredentials = _jwtSc,
            };

            var accessToken = _tokenHandler.CreateToken(userAccessToken);
            var stringAccessToken = _tokenHandler.WriteToken(accessToken);

            var userRefreshToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("UserId", session.UserID.ToString()),
                    new Claim("Data", JsonSerializer.Serialize(session.Data)),
                    new Claim("SessionId", session.SessionID.ToString()),
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.Now.AddSeconds(_jwtConfig.ExpiredRefreshKeyTimeSeconds),
                SigningCredentials = _jwtRefreshSc,
            };

            var refreshToken = _tokenHandler.CreateToken(userRefreshToken);
            var stringRefreshToken = _tokenHandler.WriteToken(refreshToken);
            return new AuthToken
            {
                AccessToken = stringAccessToken,
                RefreshToken = stringRefreshToken,
            };
        }

        public UserSession<T> GetUser<T>(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                IssuerSigningKey = _jwtRefreshSc.Key,
            };

            try
            {
                var principal = _tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
                var userIdClaim = principal.FindFirst("UserId").Value;
                var dataClaim = principal.FindFirst("Data").Value;
                var sessionIdClaim = principal.FindFirst("SessionId").Value;
                
                if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(sessionIdClaim))
                    return null;

                if (!long.TryParse(userIdClaim, out var userId) || !Guid.TryParse(sessionIdClaim, out var sessionId))
                    return null;

                var data = JsonSerializer.Deserialize<T>(dataClaim);

                return new UserSession<T>
                {
                    UserID = userId,
                    Data = data,
                    SessionID = sessionId,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
