namespace Coursework_server.Data.Models
{
    public class FieldType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();
    }
}
