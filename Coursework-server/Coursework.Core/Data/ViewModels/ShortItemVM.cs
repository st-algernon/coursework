namespace Coursework.Core.Data.ViewModels
{
    public class ShortItemVm
    { 
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? CoverUrl { get; set; }
        public Guid CollectionId { get; set; }
        public List<string> TagNames { get; set; } = new List<string>();
        public List<FullFieldVm> FullFieldVMs { get; set; } = new List<FullFieldVm>();
    }
}
