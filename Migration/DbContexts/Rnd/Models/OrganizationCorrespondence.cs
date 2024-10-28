namespace NacidScMigration.DbContexts.Rnd.Models
{
	public class OrganizationCorrespondence
	{
		public int Id { get; set; }
		public int? DistrictId { get; set; }
		public int? MunicipalityId { get; set; }
		public int? SettlementId { get; set; }
        public string Address { get; set; }
        public string AddressAlt { get; set; }
        public string WebPageUrl { get; set; }
        public int Version { get; set; }
	}
}
