namespace NacidScMigration.DbContexts.Rnd.Models
{
	public class OrganizationBasic
	{
		public int Id { get; set; }
		public int Version { get; set; }

		public string Uic { get; set; }

		public string Name { get; set; }
		public string NameAlt { get; set; }
		public string ShortName { get; set; }
		public string ShortNameAlt { get; set; }
		public bool IsActive { get; set; }
		public int? OrganizationLotId { get; set; }
		public int? OrganizationTypeId { get; set; }
		public bool IsResearchUniversity { get; set; }
	}
}
