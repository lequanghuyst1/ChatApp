using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IReactionRepository
    {
        Task<long> CreateAsync(Reaction reaction);
        Task<int> RemoveAsync(long id);
        Task<IEnumerable<Reaction>> GetByMessageIdAsync(long messageId);
    }
}