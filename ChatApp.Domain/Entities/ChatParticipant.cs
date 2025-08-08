namespace ChatApp.Domain.Entities
{
    public class ChatParticipant
    {
        public long ID { get; set; }
        public long ChatID { get; set; }
        public long UserID { get; set; }
        public string Role { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}