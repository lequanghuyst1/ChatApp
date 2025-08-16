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
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<(long id, int status)> CreateAsync(Message message)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ChatID", message.ChatID);
            parameters.Add("@SenderID", message.SenderID);
            parameters.Add("@Content", message.Content);
            parameters.Add("@MessageType", message.MessageType);
            parameters.Add("@ID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP("[dbo].[SP_Message_Create]", parameters);

            var status = parameters.Get<int>("@ResponseStatus");
            return (parameters.Get<long>("@ID"), status);
        }

        public async Task<int> UpdateAsync(Message message)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", message.ID);
            parameters.Add("@Content", message.Content);
            parameters.Add("@MessageType", message.MessageType);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
           
            await ExecuteNonQuerySP("[dbo].[SP_Message_Update]", parameters);

            return parameters.Get<int>("@ResponseStatus");
        }

        public async Task<int> DeleteAsync(long id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP("[dbo].[SP_Message_Delete]", parameters);
            return parameters.Get<int>("@ResponseStatus");
        }

        public async Task<Message> GetByIdAsync(long id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            return await GetInstanceSP<Message>("[dbo].[SP_Message_GetById]", parameters);
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(long chatId, int page, int pageSize)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ChatID", chatId);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            return await GetListSP<Message>("[dbo].[SP_Message_GetListByChatID]", parameters);
        }
    }
}
