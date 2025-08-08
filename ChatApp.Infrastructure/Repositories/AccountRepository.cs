using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Repositories;
using Dapper;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace ChatApp.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Account> GetAccountByIdAsync(long userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", userId);
                return await GetInstance<Account>("SP_Account_GetById", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get account by id", ex);
            }
        }

        public async Task<Account> AuthenticateAsync(string username, string md5Password)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);
                parameters.Add("@Password", md5Password);
                return await GetInstance<Account>("SP_Account_Authenticate", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to authenticate", ex);
            }
        }

        public async Task<long> CreateAsync(string username, string md5NewPassword, string firstName, string lastName, AccountType accountType)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", dbType: System.Data.DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Username", username);
                parameters.Add("@Password", md5NewPassword);
                parameters.Add("@FirstName", firstName);
                parameters.Add("@LastName", lastName);
                parameters.Add("@AccountType", accountType);

                await ExecuteNonQuerySP("[dbo].[SP_Account_Create]", parameters);

                var statusCode = parameters.Get<int>("@ResponseStatus");
                var id = parameters.Get<long>("@ID");

                if (statusCode != 1)
                {
                    return statusCode;
                }
                
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to register", ex);
            }
        }

        public async Task<Account> GetAccountByUsernameAsync(string username)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);
                return await GetInstance<Account>("SP_Account_GetByUsername", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get account by username", ex);
            }
        }

        public async Task<int> ResetPasswordAsync(long userId, string md5NewPassword)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ResponseStatus", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ID", userId);
                parameters.Add("@Password", md5NewPassword);

                await ExecuteNonQuerySP("SP_Account_ResetPassword", parameters);

                var statusCode = parameters.Get<int>("@ResponseStatus");

                return statusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to reset password", ex);
            }
        }
    }
}

