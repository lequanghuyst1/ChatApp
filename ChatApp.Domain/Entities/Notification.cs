using System;

namespace ChatApp.Domain.Entities
{
    public class Notification
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long SenderID { get; set; }
        public long ChatID { get; set; }
        public byte Type { get; set; } // e.g., 1: Message, 2: Friend Request, etc.
        public string Content { get; set; } = string.Empty;
        public bool? IsRead { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}