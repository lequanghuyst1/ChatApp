using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ChatApp.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public UserSession<T> GetUser<T>()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                return null;
            }
            var userIdClaim = user.FindFirst("UserId").Value;
            var sessionIdClaim = user.FindFirst("SessionId").Value;
            var userDataClaim = user.FindFirst("Data").Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(sessionIdClaim) || string.IsNullOrEmpty(userDataClaim))
            {
                return null;
            }

            if (!long.TryParse(userIdClaim, out var userId) || !Guid.TryParse(sessionIdClaim, out var sessionId))
            {
                return null;
            }

            T data = JsonSerializer.Deserialize<T>(userDataClaim);

            return new UserSession<T>{
                UserID = userId,
                SessionID = sessionId,
                Data = data,
            };
        }

        public UserSession GetUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                return null;

            var userIdClaim = user.FindFirst("UserId")?.Value;
            var sessionIdClaim = user.FindFirst("SessionId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(sessionIdClaim))
                return null;

            if (!long.TryParse(userIdClaim, out var userId) || !Guid.TryParse(sessionIdClaim, out var sessionId))
                return null;

            return new UserSession
            {
                UserID = userId,
                SessionID = sessionId
            };
        }
    }
}
