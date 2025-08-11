using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Interfaces
{
    public interface IFriendRepository
    {
        Task<(long newId, int status)> AddFriendRequestAsync(long userId, long friendId);
        Task<int> UpdateFriendStatusAsync(long userId, long friendId, FriendStatus status);
        Task<IEnumerable<UserFriend>> GetListByUserAsync(long userId);
        Task<IEnumerable<UserFriend>> GetListFriendRequestAsync(long userId);
        Task<int> DeleteAsync(long userId, long friendId, string username);
        Task<int> BlockAsync(long userId, long friendId, string username);
        Task<int> UnblockAsync(long userId, long friendId, string username);
        Task<int> AcceptAsync(long userId, long friendId, string username);
        Task<int> RejectAsync(long userId, long friendId, string username);
    }
}