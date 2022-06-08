namespace Coursework_server.Data.Models
{
    public class FieldItem
    {
        public Guid FieldId { get; set; }
        public Field Field { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public string? Value { get; set; }
    }
}
