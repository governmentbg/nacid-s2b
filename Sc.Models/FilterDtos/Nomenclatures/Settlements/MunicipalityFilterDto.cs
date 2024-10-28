using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.FilterDtos.Base.Nomenclatures;

namespace Sc.Models.Filters.Nomenclatures.Settlements
{
    public class MunicipalityFilterDto : NomenclatureFilterDto<Municipality>
    {
        public int? DistrictId { get; set; }

        public override IQueryable<Municipality> WhereBuilder(IQueryable<Municipality> query)
        {
            if (DistrictId.HasValue)
            {
                query = query.Where(e => e.DistrictId == DistrictId);
            }

            return base.WhereBuilder(query);
        }
    }
}
