using Sc.Models.Entities.Base;
using Sc.Models.Enums.Complexes;

namespace Sc.Models.Dtos.Nomenclatures.Complexes
{
    public class ComplexOrganizationDto : Entity
    {
        public int ComplexId { get; set; }

        public OrganizationTypeEnum? OrganizationTypeEnum { get; set; }

        public int? FinancingOrganizationLotId { get; set; }
        public string FinancingOrganizationName { get; set; }
        public string FinancingOrganizationNameAlt { get; set; }
        public string FinancingOrganizationShortName { get; set; }
        public string FinancingOrganizationShortNameAlt { get; set; }

        public int? OrganizationLotId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationNameAlt { get; set; }
        public string OrganizationShortName { get; set; }
        public string OrganizationShortNameAlt { get; set; }

        public string NameRND { get; set; }
        public string NameRNDAlt { get; set; }
    }
}
