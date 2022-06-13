namespace Coursework.Core.Data.ViewModels
{
    public class ItemVm
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid CollectionId { get; set; }
        public UsersItemVm UsersItemVm { get; set; }
        public List<TagVm> TagVMs { get; set; } = new List<TagVm>();
        public List<FullFieldVm> FullFieldVMs { get; set; } = new List<FullFieldVm>();
        public Guid OwnerId { get; set; }
    }
}
