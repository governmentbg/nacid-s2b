using Sc.Models.Dtos.Base.Nomenclatures;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Enums.Institutions;

namespace Sc.Models.Dtos.Nomenclatures.Institutions
{
    public class InstitutionDto : NomenclatureHierarchyDto<InstitutionDto>
    {
        public int LotNumber { get; set; }

        public string Uic { get; set; }
        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }

        public OrganizationType? OrganizationType { get; set; }
        public OwnershipType? OwnershipType { get; set; }

        public int? SettlementId { get; set; }
        public SettlementDto Settlement { get; set; }
        public int? MunicipalityId { get; set; }
        public MunicipalityDto Municipality { get; set; }
        public int? DistrictId { get; set; }
        public DistrictDto District { get; set; }
        public string Address { get; set; }
        public string AddressAlt { get; set; }
        public string WebPageUrl { get; set; }

        public List<InstitutionDto> Children { get; set; } = new List<InstitutionDto>();
    }
}
