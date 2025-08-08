using System;

namespace ChatApp.Application.DTOs
{
    public class ReactionDTO
    {
        public long ID { get; set; }
        public long MessageID { get; set; }
        public long UserID { get; set; }
        public string Emoji { get; set; } = string.Empty;
        public DateTime? Timestamp { get; set; }
    }
}