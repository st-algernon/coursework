namespace Coursework.Core.Data.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
