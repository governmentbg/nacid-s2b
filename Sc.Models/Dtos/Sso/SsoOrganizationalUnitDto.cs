using Sc.Models.Dtos.Nomenclatures.Settlements;

namespace Sc.Models.Dtos.Sso
{
    public class SsoOrganizationalUnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public string Alias { get; set; }
        public int? ExternalId { get; set; }

        public int? SettlementId { get; set; }
        public SettlementDto Settlement { get; set; }
        public int? MunicipalityId { get; set; }
        public MunicipalityDto Municipality { get; set; }
        public int? DistrictId { get; set; }
        public DistrictDto District { get; set; }
    }
}
