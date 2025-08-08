using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetByIdAsync(long id);
        Task<IEnumerable<Chat>> GetListByUserAsync(long userId);
        Task<long> CreateAsync(Chat chat, long userId, string userName);
        Task<int> UpdateAsync(Chat chat, long userId, string userName);
        Task<int> DeleteAsync(long id);
    }
}