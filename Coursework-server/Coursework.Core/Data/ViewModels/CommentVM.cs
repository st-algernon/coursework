namespace Coursework.Core.Data.ViewModels
{
    public class CommentVm
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public UserVm AuthorVm { get; set; }
        public Guid ItemId { get; set; }
    }
}
