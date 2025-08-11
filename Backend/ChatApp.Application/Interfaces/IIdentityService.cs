using ChatApp.Application.Model;

namespace ChatApp.Application.Interfaces
{
    public interface IIdentityService
    {
        UserSession<T> GetUser<T>();
        UserSession GetUser();
    }
}
