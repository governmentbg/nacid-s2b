namespace Infrastructure
{
    public class OrganizationalUnitContext
    {
        public int OrganizationalUnitId { get; set; }
        public string Alias { get; set; }
        public int? ExternalId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();

        public List<OrganizationalUnitContext> Predecessors { get; set; } = new List<OrganizationalUnitContext>();
        public List<OrganizationalUnitContext> Successors { get; set; } = new List<OrganizationalUnitContext>();

        // Custom field from NacidSc only if user is for institution or complex
        public int? SupplierId { get; set; }

        // Custom field from NacidSc Company model else is always true
        public bool IsActive { get; set; } = true;
    }
}
