using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Repositories;
using Dapper;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ChatApp.Infrastructure.Repositories
{
    public class ChatParticipantRepository : BaseRepository, IChatParticipantRepository
    {
        public ChatParticipantRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<(long id, int status)> AddAsync(ChatParticipant participant)
        {
            var procName = "SP_ChatParticipant_Create";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ChatID", participant.ChatID);
            parameters.Add("@UserID", participant.UserID);
            parameters.Add("@Role", participant.Role);
            parameters.Add("@ID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP(procName, parameters);

            return (parameters.Get<long>("@ID"), parameters.Get<int>("@ResponseStatus"));
        }

        public async Task<IEnumerable<ChatParticipant>> GetListByChatIdAsync(long chatId)
        {
            var procName = "SP_ChatParticipant_GetListByChatId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ChatID", chatId);
            var result = await GetListSP<ChatParticipant>(procName, parameters);
            return result;
        }

        public async Task<int> RemoveAsync(long chatId, long userId)
        {
            var procName = "SP_ChatParticipant_Delete";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ChatID", chatId);
            parameters.Add("@UserID", userId);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            
            await ExecuteNonQuerySP(procName, parameters);

            var status = parameters.Get<int>("@ResponseStatus");
            
            return status;
        }
    }
}
