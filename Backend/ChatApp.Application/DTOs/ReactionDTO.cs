using System;

namespace ChatApp.Application.DTOs
{
    public class ReactionDTO
    {
        public long ID { get; set; }
        public long MessageID { get; set; }
        public long SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderAvatar { get; set; }
        public string Emoji { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}