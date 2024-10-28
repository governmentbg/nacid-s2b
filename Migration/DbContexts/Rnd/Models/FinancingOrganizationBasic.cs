namespace Migration.DbContexts.Rnd.Models
{
    public class FinancingOrganizationBasic
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }

        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }
    }
}
