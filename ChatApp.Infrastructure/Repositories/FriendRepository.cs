using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Repositories;
using Dapper;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Repositories
{
    public class FriendRepository : BaseRepository, IFriendRepository
    {
        public FriendRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<(long, int)> AddFriendRequestAsync(long userId, long friendId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);
            parameters.Add("@FriendID", friendId);
            parameters.Add("@ID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP("[dbo].[SP_Friend_Create]", parameters);

            return (parameters.Get<long>("@ID"), parameters.Get<int>("@ResponseStatus"));
        }

        public async Task<int> UpdateFriendStatusAsync(long userId, long friendId, FriendStatus status)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);
            parameters.Add("@FriendID", friendId);
            parameters.Add("@Status", status);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP("[dbo].[SP_Friend_UpdateStatus]", parameters);

            return parameters.Get<int>("@ResponseStatus");
        }

        public async Task<IEnumerable<UserFriend>> GetListByUserAsync(long userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);
            var result = await GetListSP<UserFriend>("[dbo].[SP_Friend_GetListByUser]", parameters);
            return result;
        }

        public async Task<IEnumerable<UserFriend>> GetListFriendRequestAsync(long userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);
            return await GetListSP<UserFriend>("[dbo].[SP_Friend_GetListFriendRequest]", parameters);
        }

        public async Task<int> RemoveFriendAsync(long userId, long friendId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);
            parameters.Add("@FriendID", friendId);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP("[dbo].[SP_Friend_Delete]", parameters);
            return parameters.Get<int>("@ResponseStatus");
        }
    }
}
