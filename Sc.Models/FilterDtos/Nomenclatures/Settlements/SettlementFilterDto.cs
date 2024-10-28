using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.FilterDtos.Base.Nomenclatures;

namespace Sc.Models.Filters.Nomenclatures.Settlements
{
    public class SettlementFilterDto : NomenclatureFilterDto<Settlement>
    {
        public int? DistrictId { get; set; }
        public int? MunicipalityId { get; set; }

        public override IQueryable<Settlement> WhereBuilder(IQueryable<Settlement> query)
        {
            if (DistrictId.HasValue)
            {
                query = query.Where(e => e.DistrictId == DistrictId);
            }

            if (MunicipalityId.HasValue)
            {
                query = query.Where(e => e.MunicipalityId == MunicipalityId);
            }

            return base.WhereBuilder(query);
        }
    }
}
