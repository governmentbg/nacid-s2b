namespace Migration.DbContexts.Rnd.Models
{
    public class ComplexOrganizationPart
    {
        public int Id { get; set; }

        public int CommitId { get; set; }

        public int EntityId { get; set; }
        public ComplexOrganization Entity { get; set; }

        public int? State { get; set; }

        public int? InitialPartId { get; set; }
    }
}
