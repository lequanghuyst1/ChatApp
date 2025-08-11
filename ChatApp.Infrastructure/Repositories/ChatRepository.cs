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

        public async Task<(long id, int status)> CreateAsync(Chat chat)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Type", chat.Type);
            parameters.Add("@Title", chat.Title);
            parameters.Add("@CreatedBy", chat.CreatedBy);
            parameters.Add("@CreatedByName", chat.CreatedByName);
            parameters.Add("@ID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
           
            await ExecuteNonQuerySP("[dbo].[SP_Chat_Create]", parameters);

            return (parameters.Get<long>("@ID"), parameters.Get<int>("@ResponseStatus"));
        }

        public async Task<int> UpdateAsync(Chat chat)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ID", chat.ID);
            parameters.Add("@Type", chat.Type);
            parameters.Add("@Title", chat.Title);
            parameters.Add("@   ", chat.UpdatedBy);
            parameters.Add("@UpdatedByName", chat.UpdatedByName);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            
            await ExecuteNonQuerySP("[dbo].[SP_Chat_Update]", parameters);

            var status = parameters.Get<int>("@ResponseStatus");
            
            return status;
        }

        public async Task<int> LeaveAsync(long chatId, long userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ChatID", chatId);
            parameters.Add("@UserID", userId);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            
            await ExecuteNonQuerySP("[dbo].[SP_Chat_Leave]", parameters);

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
