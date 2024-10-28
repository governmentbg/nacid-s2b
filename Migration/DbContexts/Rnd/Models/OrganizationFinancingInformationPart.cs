namespace NacidScMigration.DbContexts.Rnd.Models
{
	public class OrganizationFinancingInformationPart
	{
		public int Id { get; set; }

		public int EntityId { get; set; }
		public OrganizationFinancingInformation Entity { get; set; }

		public int? State { get; set; }
	}
}
