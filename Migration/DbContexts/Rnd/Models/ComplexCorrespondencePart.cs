namespace Migration.DbContexts.Rnd.Models
{
    public class ComplexCorrespondencePart
    {
        public int Id { get; set; }

        public int EntityId { get; set; }
        public ComplexCorrespondence Entity { get; set; }

        public int? State { get; set; }
    }
}
