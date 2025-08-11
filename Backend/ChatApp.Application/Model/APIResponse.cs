using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Model
{
    public class APIResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public static APIResponse Success(string message = "Success")
        {
            return new APIResponse
            {
                Code = 1,
                Message = message
            };
        }

        public static APIResponse Failure(int code, string message)
        {
            return new APIResponse
            {
                Code = code,
                Message = message
            };
        }
    }

    public class APIResponse<T> : APIResponse
    {
        public T Data { get; set; }

        public static APIResponse<T> Success(T data, string message = "Success")
        {
            return new APIResponse<T>
            {
                Code = 1,
                Message = message,
                Data = data
            };
        }
    }

}
