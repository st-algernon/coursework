namespace Coursework_server.Data.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? AuthorId { get; set; }
        public User Author { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}