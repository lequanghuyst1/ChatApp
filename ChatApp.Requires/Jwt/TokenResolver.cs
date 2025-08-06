using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ChatApp.Requires.Jwt
{
    public class TokenResolver
    {
        private IHttpContextAccessor _context;

        public TokenResolver(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public UserSession<T> Get<T>()
        {
            try
            {
                if (_context.HttpContext.User != null 
                    && _context.HttpContext.User.Identity != null 
                    && _context.HttpContext.User.Identity.IsAuthenticated)
                {
                    string userId = _context.HttpContext.User.FindFirst("UserId")?.Value;
                    string sessionId = _context.HttpContext.User.FindFirst("SessionId")?.Value;

                    return new UserSession<T>
                    {
                        Data = JsonSerializer.Deserialize<T>(_context.HttpContext.User.FindFirst("Data")?.Value),
                        UserID = long.Parse(userId),
                        SessionID = Guid.Parse(sessionId),
                    };
                }
                else
                {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
        }
    }
}
