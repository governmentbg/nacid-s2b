namespace Infrastructure
{
    public class UserContext
    {
        public string ClientId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public List<OrganizationalUnitContext> OrganizationalUnits { get; set; } = new List<OrganizationalUnitContext>();

        // If company user
        public int? CompanyId { get; set; }

        public UserContext()
        {
            ClientId = null;
            UserId = null;
            UserName = string.Empty;
            FullName = string.Empty;
            PhoneNumber = null;
            CompanyId = null;
        }
    }
}
