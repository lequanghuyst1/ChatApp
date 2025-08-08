namespace ChatApp.Domain.Entities
{
    public class Reaction
    {
        public long ID { get; set; }
        public long MessageID { get; set; }
        public long UserID { get; set; }
        public string Emoji { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}