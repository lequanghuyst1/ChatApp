using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Repositories
{
    public interface IProfileRepository
    {
        Task<UserProfile> GetProfileAsync(long id);
        Task<int> UpdateProfileAsync(long userId, UserProfile profile);
        Task<int> UpdateAvatarAsync(long userId, string avatar);
        Task<int> UpdateBioAsync(long userId, string bio);
    }
}