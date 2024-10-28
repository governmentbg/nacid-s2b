namespace NacidScMigration.DbContexts.Rnd.Models
{
	public class OrganizationType
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string NameAlt { get; set; }
		public bool IsActive { get; set; }
		public int? ViewOrder { get; set; }
		public int Version { get; set; }
	}
}
