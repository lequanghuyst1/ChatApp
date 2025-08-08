using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IChatParticipantRepository
    {
       Task<long> CreateAsync(ChatParticipant participant);
       Task<int> RemoveAsync(long chatId, long userId);
       Task<IEnumerable<ChatParticipant>> GetListByChatIdAsync(long chatId);
    }
}