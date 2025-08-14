namespace ChatApp.Domain.Entities
{
    public class UserProfile
    {
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string Bio { get; set; }
    }
}