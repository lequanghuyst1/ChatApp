using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class UserFriend
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long FriendID { get; set; }
        public FriendStatus Status { get; set; }
        public DateTime AddedAt { get; set; }
    }
}