namespace NacidScMigration.DbContexts.Rnd.Models
{
	public class OrganizationLot
	{
		public int Id { get; set; }
		public int Version { get; set; }

		public int Number { get; set; }
		public int? ParentLotId { get; set; }
		public int? _ACADUniId { get; set; }
		public int? _ACADFacultyId { get; set; }
	}
}
