namespace Sc.Models.Dtos.Base.FileStorages
{
    public abstract class ScAttachedFileDto
    {
        public int Id { get; set; }
        public Guid Key { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public int DbId { get; set; }
    }
}
