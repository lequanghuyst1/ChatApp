
using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Account
{
    public class AccountDTO
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