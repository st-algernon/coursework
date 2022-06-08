using Coursework_server.Data.Models;

namespace Coursework_server.Data.Services
{
    public class TagService
    {
        private readonly AppDbContext _db;

        public TagService(AppDbContext context)
        {
            _db = context;
        }
        public List<Tag> SearchTags(string query) 
            => _db.Tags.Where(t => t.Name.Contains(query)).ToList();

        public List<Tag> GetTagsOrCreate(List<string> tagNames)
        {
            var tags = new List<Tag>();

            foreach (var name in tagNames)
            {
                var tag = _db.Tags.FirstOrDefault(t => t.Name == name);

                if (tag == null)
                {
                    tag = new Tag
                    {
                        Name = name
                    };

                    _db.Tags.Add(tag);
                }

                tags.Add(tag);
            }

            return tags;
        }

        public List<Tag> GetTopTags() => _db.Tags.OrderByDescending(t => t.Items.Count).ToList();
    }
}
