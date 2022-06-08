namespace Coursework_server.Data.ViewModels
{
    public class ShortCollectionVm
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string TopicName { get; set; }
        public Guid OwnerId { get; set; }
    }
}
