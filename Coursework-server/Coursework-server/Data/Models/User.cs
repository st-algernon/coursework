namespace Coursework_server.Data.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    public enum UserState
    {
        Active,
        Blocked
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }
        public UserState UserState { get; set; }
        public List<Collection> Collections { get; set; } = new List<Collection>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<UserItem> UserItems { get; set; } = new List<UserItem>();
    }
}
