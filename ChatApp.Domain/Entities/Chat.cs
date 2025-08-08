using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class Chat
    {
        public long ID { get; set; }
        public ChatType Type { get; set; }
        public string Title { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public long UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}