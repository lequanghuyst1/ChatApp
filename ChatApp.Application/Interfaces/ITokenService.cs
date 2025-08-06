using ChatApp.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Interfaces
{
    public interface ITokenService
    {
        AuthToken GenerateToken<T>(UserSession<T> user);
        UserSession<T> GetUser<T>(string refreshToken);
    }
}
