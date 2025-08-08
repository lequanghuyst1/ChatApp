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
        Task<int> RemoveFriendAsync(long userId, long friendId);
    }
}