namespace NacidScMigration.DbContexts.Rnd.Models
{
	public class OrganizationCorrespondencePart
	{
		public int Id { get; set; }

		public int EntityId { get; set; }
		public OrganizationCorrespondence Entity { get; set; }

		public int? State { get; set; }
	}
}
