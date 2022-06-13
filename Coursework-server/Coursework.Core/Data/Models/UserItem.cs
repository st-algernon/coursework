namespace Coursework.Core.Data.Models
{
    public class UserItem
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public bool IsLiked { get; set; }
    }
}
