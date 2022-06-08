namespace Coursework_server.Data.Models
{
    public class Field
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid FieldTypeId { get; set; }
        public FieldType FieldType { get; set; }
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        public List<FieldItem> FieldItems { get; set; } = new List<FieldItem>();
    }
}
