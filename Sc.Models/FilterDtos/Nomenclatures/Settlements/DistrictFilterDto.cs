using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.FilterDtos.Base.Nomenclatures;

namespace Sc.Models.Filters.Nomenclatures.Settlements
{
    public class DistrictFilterDto : NomenclatureFilterDto<District>
    {
        public int? SmartSpecializationId { get; set; }

        public override IQueryable<District> WhereBuilder(IQueryable<District> query)
        {
            if (SmartSpecializationId.HasValue)
            {
                query = query.Where(e => 
                    e.Institutions.Any(s => s.Suppliers.Any(m => m.SupplierOfferings.Any(f => f.SmartSpecializations.Any(j => j.SmartSpecializationId == SmartSpecializationId))))
                    || e.Complexes.Any(s => s.Suppliers.Any(m => m.SupplierOfferings.Any(f => f.SmartSpecializations.Any(j => j.SmartSpecializationId == SmartSpecializationId)))));
            }

            return base.WhereBuilder(query);
        }
    }
}
