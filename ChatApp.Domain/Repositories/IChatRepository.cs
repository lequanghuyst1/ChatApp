using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetByIdAsync(long id);
        Task<IEnumerable<Chat>> GetListByUserAsync(long userId);
        Task<(long id, int status)> CreateAsync(Chat chat);
        Task<int> UpdateAsync(Chat chat);
        Task<int> DeleteAsync(long id);
    }
}