using Sc.Models.Dtos.Base.Nomenclatures;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Enums.Complexes;

namespace Sc.Models.Dtos.Nomenclatures.Complexes
{
    public class ComplexDto : NomenclatureDto
    {
        public int LotNumber { get; set; }

        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }

        public string CoordinatorPosition { get; set; }
        public string CoordinatorPositionAlt { get; set; }
        public string Category { get; set; }
        public string CategoryAlt { get; set; }
        public string Benefits { get; set; }
        public string BenefitsAlt { get; set; }
        public string ScientificTeam { get; set; }
        public string ScientificTeamAlt { get; set; }

        public AreaOfActivity? AreaOfActivity { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string EuropeanInfrastructure { get; set; }

        public bool IsForeign { get; set; }
        public int? SettlementId { get; set; }
        public SettlementDto Settlement { get; set; }
        public int? MunicipalityId { get; set; }
        public MunicipalityDto Municipality { get; set; }
        public int? DistrictId { get; set; }
        public DistrictDto District { get; set; }
        public string ForeignSettlement { get; set; }
        public string ForeignSettlementAlt { get; set; }
        public string Address { get; set; }
        public string AddressAlt { get; set; }

        public string WebPageUrl { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public List<ComplexOrganizationDto> ComplexOrganizations { get; set; } = new List<ComplexOrganizationDto>();
    }
}
