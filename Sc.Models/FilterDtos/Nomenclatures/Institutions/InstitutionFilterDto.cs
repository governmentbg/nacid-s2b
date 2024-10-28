using Sc.Models.Entities.Nomenclatures.Institutions;
using Sc.Models.Enums.Institutions;
using Sc.Models.FilterDtos.Base.Nomenclatures;

namespace Sc.Models.FilterDtos.Nomenclatures.Institutions
{
    public class InstitutionFilterDto : NomenclatureHierarchyFilterDto<Institution>
    {
        public string Uic { get; set; }
        public int? DistrictId { get; set; }
        public int? MunicipalityId { get; set; }
        public int? SettlementId { get; set; }
        public OrganizationType? OrganizationType { get; set; }
        public List<OrganizationType> OrganizationTypes { get; set; } = new List<OrganizationType>();
        public OwnershipType? OwnershipType { get; set; }

        public override IQueryable<Institution> WhereBuilder(IQueryable<Institution> query)
        {
            if (!string.IsNullOrWhiteSpace(Uic))
            {
                var uic = Uic.Trim().ToLower();
                query = query.Where(e => e.Uic.Trim().ToLower() == uic);
            }

            if (DistrictId.HasValue)
            {
                query = query.Where(e => e.DistrictId == DistrictId);
            }

            if (MunicipalityId.HasValue)
            {
                query = query.Where(e => e.MunicipalityId == MunicipalityId);
            }

            if (SettlementId.HasValue)
            {
                query = query.Where(e => e.SettlementId == SettlementId);
            }

            if (OrganizationType.HasValue)
            {
                query = query.Where(e => e.OrganizationType == OrganizationType);
            }

            if (OrganizationTypes != null && OrganizationTypes.Any())
            {
                query = query.Where(e => OrganizationTypes.Contains(e.OrganizationType.Value));
            }

            if (OwnershipType.HasValue)
            {
                query = query.Where(e => e.OwnershipType == OwnershipType);
            }

            return base.WhereBuilder(query);
        }
    }
}
