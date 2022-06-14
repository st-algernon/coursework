namespace Coursework.Core.Data.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<FieldItem> ItemFields { get; set; } = new List<FieldItem>();
        public List<UserItem> ItemUsers { get; set; } = new List<UserItem>();
    }
}
