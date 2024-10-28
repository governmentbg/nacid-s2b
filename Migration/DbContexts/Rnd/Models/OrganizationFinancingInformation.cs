using Sc.Models.Enums.Institutions;

namespace NacidScMigration.DbContexts.Rnd.Models
{
    public class OrganizationFinancingInformation
	{
		public int Id { get; set; }
		public OwnershipType? OwnershipTypeId { get; set; }
		public int Version { get; set; }
	}
}
