using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message> GetByIdAsync(long id);
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(long chatId, int page, int pageSize);
        Task<(long id, int status)> CreateAsync(Message message);
        Task<int> UpdateAsync(Message message);
        Task<int> DeleteAsync(long id);
    }
}