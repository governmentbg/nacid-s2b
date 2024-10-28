namespace Migration.DbContexts.Rnd.Models
{
    public class FinancingOrganizationBasicPart
    {
        public int Id { get; set; }

        public int EntityId { get; set; }
        public FinancingOrganizationBasic Entity { get; set; }

        public int? State { get; set; }
    }
}
