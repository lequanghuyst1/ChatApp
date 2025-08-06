using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class Account
    {
        public long UserID { get; set; }
        public string Username { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastSeen { get; set; }
    }
}