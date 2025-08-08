using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs
{
    public class MessageDTO
    {
        public long ID { get; set; }
        public long ChatID { get; set; }
        public long SenderID { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string ReadBy { get; set; }
        public IEnumerable<ReactionDTO> Reactions { get; set; } = new List<ReactionDTO>();
    }
}