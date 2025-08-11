using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByIdAsync(long userId);
        Task<Account> GetAccountByUsernameAsync(string username);
        Task<Account> AuthenticateAsync(string username, string md5Password);
        Task<int> ResetPasswordAsync(long userId, string md5NewPassword);
        Task<long> CreateAsync(string username, string md5NewPassword, string firstName, string lastName, AccountType accountType);
        Task<int> UpdateAsync(Account account);
    }
}