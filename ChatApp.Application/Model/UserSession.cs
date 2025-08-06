using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Model
{
    public class UserSession
    {
        public long UserID { get; set; }
        public Guid SessionID { get; set; }
    }

    public class UserSession<T> : UserSession
    {
        public T Data { get; set; }
    }
}
