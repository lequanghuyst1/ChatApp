using ChatApp.Domain.Entities;
using Dapper;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Infrastructure.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public NotificationRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<long> AddAsync(Notification notification)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ChatID", notification.ChatID);
            parameters.Add("@SenderID", notification.SenderID);
            parameters.Add("@Content", notification.Content);
            parameters.Add("@Type", notification.Type);
            parameters.Add("@CreatedAt", notification.CreatedAt);
            parameters.Add("@ID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP("[dbo].[SP_Notification_Create]", parameters);
            return parameters.Get<long>("@ID");
        }

        public Task<IEnumerable<Notification>> GetListByUserAsync(long userId, bool unreadOnly = false)
        {
            throw new NotImplementedException();
        }

        public async Task<int> MarkAsReadAsync(long id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            parameters.Add("@ResponseStatus", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await ExecuteNonQuerySP("[dbo].[SP_Notification_MarkAsRead]", parameters);
            return parameters.Get<int>("@ResponseStatus");
        }
    }
}
