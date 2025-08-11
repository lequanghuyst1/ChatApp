using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class UserFriend
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long FriendID { get; set; }
        public string FriendName { get; set; }
        public string FriendAvatar { get; set; }
        public FriendStatus Status { get; set; }
        public DateTime AddedAt { get; set; }
    }
}