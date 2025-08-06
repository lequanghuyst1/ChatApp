using ChatApp.Application.Interfaces;
using ChatApp.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Services
{
    public class IdentityService : IIdentityService
    {
        public UserSession<T> GetUser<T>()
        {
            throw new NotImplementedException();
        }

        public UserSession GetUser()
        {
            throw new NotImplementedException();
        }
    }
}
