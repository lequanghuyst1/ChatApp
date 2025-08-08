using ChatApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<long> AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetListByUserAsync(long userId, bool unreadOnly = false);
        Task<int> MarkAsReadAsync(long id);
    }
}