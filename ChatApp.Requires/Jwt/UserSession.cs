using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Requires.Jwt
{
    public class UserSession<T>
    {
        public long UserID { get; set; }
        public T Data { get; set; }
        public Guid SessionID { get; set; }
    }
}
