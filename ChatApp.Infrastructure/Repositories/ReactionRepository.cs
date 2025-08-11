using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Domain.Repositories;
using Dapper;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ChatApp.Infrastructure.Repositories
{
    public class ReactionRepository : BaseRepository, IReactionRepository
    {
        public ReactionRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<(long id, int status)> CreateAsync(Reaction reaction)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@MessageID", reaction.MessageID);
            parameters.Add("@SenderID", reaction.SenderID);
            parameters.Add("@Emoji", reaction.Emoji);
            parameters.Add("@ID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
           
            await ExecuteNonQuerySP("[dbo].[SP_Reaction_Create]", parameters);

            return (parameters.Get<long>("@ID"), parameters.Get<int>("@ResponseStatus"));
        }

        public async Task<int> UpdateAsync(Reaction reaction)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", reaction.ID);
            parameters.Add("@SenderID", reaction.SenderID);
            parameters.Add("@Emoji", reaction.Emoji);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
           
            await ExecuteNonQuerySP("[dbo].[SP_Reaction_Update]", parameters);

            return parameters.Get<int>("@ResponseStatus");
        }

        public async Task<int> RemoveAsync(long id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP("[dbo].[SP_Reaction_Delete]", parameters);
            return parameters.Get<int>("@ResponseStatus");
        }

        public async Task<IEnumerable<Reaction>> GetByMessageIdAsync(long messageId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@MessageID", messageId);
            var result = await GetListSP<Reaction>("[dbo].[SP_Reaction_GetByMessageId]", parameters);
            return result;
        }
    }
}
