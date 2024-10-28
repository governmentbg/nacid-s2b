namespace NacidScMigration.DbContexts.Rnd.Models
{
	public class OrganizationBasicPart
	{
		public int Id { get; set; }

		public int EntityId { get; set; }
		public OrganizationBasic Entity { get; set; }

		public int? State { get; set; }
	}
}
