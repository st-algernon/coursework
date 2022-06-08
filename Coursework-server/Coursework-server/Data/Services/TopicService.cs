using Coursework_server.Data.Models;

namespace Coursework_server.Data.Services
{
    public class TopicService
    {
        private readonly AppDbContext _db;

        public TopicService(AppDbContext context)
        {
            _db = context;
        }

        public List<Topic> GetTopics() => _db.Topics.ToList();
    }
}
