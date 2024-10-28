namespace Migration.DbContexts.Rnd.Models
{
    public class ComplexCorrespondence
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public bool IsForeign { get; set; }

        public int? CountryId { get; set; }

        public int? DistrictId { get; set; }

        public int? MunicipalityId { get; set; }

        public int? SettlementId { get; set; }

        public string ForeignSettlement { get; set; }
        public string ForeignSettlementAlt { get; set; }

        public string Address { get; set; }
        public string AddressAlt { get; set; }
        public string WebPageUrl { get; set; }
        public string PostCode { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}
