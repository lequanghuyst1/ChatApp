using ChatApp.Domain.Repositories;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using ChatApp.Domain.Entities;
using Dapper;
using System.Data;
namespace ChatApp.Infrastructure.Repositories
{

    public class ProfileRepository : BaseRepository, IProfileRepository
    {

        public ProfileRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<UserProfile> GetProfileAsync(long id)
        {
           try
           {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id);
                return await GetInstance<UserProfile>("SP_Profile_GetById", parameters);
           }
           catch (Exception ex)
           {
                throw new Exception("Failed to get profile", ex);
           }
        }

        public async Task<int> UpdateAvatarAsync(long userId, string avatar)
        {
           try
           {
                var parameters = new DynamicParameters();
                parameters.Add("@ResponseStatus", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ID", userId);
                parameters.Add("@Avatar", avatar);

                await ExecuteNonQuerySP("SP_Profile_UpdateAvatar", parameters);

                var statusCode = parameters.Get<int>("@ResponseStatus");

                return statusCode;
           }
           catch (Exception ex)
           {
                throw new Exception("Failed to update avatar", ex);
           }
        }

        public async Task<int> UpdateBioAsync(long userId, string bio)
        {
           try
           {
                var parameters = new DynamicParameters();
                parameters.Add("@ResponseStatus", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ID", userId);
                parameters.Add("@Bio", bio);

                await ExecuteNonQuerySP("SP_Profile_UpdateBio", parameters);

                var statusCode = parameters.Get<int>("@ResponseStatus");

                return statusCode;
           }
           catch (Exception ex)
           {
                throw new Exception("Failed to update bio", ex);
           }
        }

        public async Task<int> UpdateProfileAsync(long userId, UserProfile profile)
        {
           try
           {
                var parameters = new DynamicParameters();
                parameters.Add("@ResponseStatus", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ID", userId);
                parameters.Add("@FirstName", profile.FirstName);
                parameters.Add("@LastName", profile.LastName);
                parameters.Add("@Avatar", profile.Avatar);
                parameters.Add("@DateOfBirth", profile.DateOfBirth);
                parameters.Add("@Phone", profile.Phone);
                parameters.Add("@Email", profile.Email);
                parameters.Add("@Gender", profile.Gender);
                parameters.Add("@Bio", profile.Bio);

                await ExecuteNonQuerySP("SP_Profile_UpdateProfile", parameters);

                var statusCode = parameters.Get<int>("@ResponseStatus");

                return statusCode;
           }
           catch (Exception ex)
           {
                throw new Exception("Failed to update profile", ex);
           }
        }
    }
}
