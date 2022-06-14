namespace Coursework.Core.Data.Models
{
	public class Collection
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string? CoverUrl { get; set; }
		public Guid TopicId { get; set; }
		public Topic Topic { get; set; }
		public Guid OwnerId { get; set; }
		public User Owner { get; set; }
		public List<Field> Fields { get; set; } = new List<Field>();
		public List<Item> Items { get; set; } = new List<Item>();
	}
}