using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Repositories;
using Dapper;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ChatApp.Infrastructure.Repositories
{
    public class ChatRepository : BaseRepository, IChatRepository
    {
        public ChatRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<long> CreateAsync(Chat chat, long userId, string userName)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Type", chat.Type);
            parameters.Add("@Title", chat.Title);
            parameters.Add("@CreatedBy", userId);
            parameters.Add("@CreatedByName", userName);
            parameters.Add("@ID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
           
            await ExecuteNonQuerySP("[dbo].[SP_Chat_Create]", parameters);

            var status = parameters.Get<int>("@ResponseStatus");
            if(status != 1){
                return status;
            }
            return parameters.Get<long>("@ID");
        }

        public async Task<int> UpdateAsync(Chat chat, long userId, string userName)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ID", chat.ID);
            parameters.Add("@Type", chat.Type);
            parameters.Add("@Title", chat.Title);
            parameters.Add("@UpdatedBy", userId);
            parameters.Add("@UpdatedByName", userName);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            
            await ExecuteNonQuerySP("[dbo].[SP_Chat_Update]", parameters);

            var status = parameters.Get<int>("@ResponseStatus");
            
            return status;
        }

        public async Task<int> DeleteAsync(long id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            
            await ExecuteNonQuerySP("[dbo].[SP_Chat_Delete]", parameters);

            var status = parameters.Get<int>("@ResponseStatus");
            
            return status;
        }

        public async Task<Chat> GetByIdAsync(long id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            var result = await GetInstanceSP<Chat>("[dbo].[SP_Chat_GetById]", parameters);
            return result;
        }

        public async Task<IEnumerable<Chat>> GetListByUserAsync(long userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", userId);
            var result = await GetListSP<Chat>("[dbo].[SP_Chat_GetListByUser]", parameters);
            return result;
        }
    }
}
