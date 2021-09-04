namespace TFW.Framework.WebExamples.Entities
{
    public class FileEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public byte[] Content { get; set; }
    }
}
