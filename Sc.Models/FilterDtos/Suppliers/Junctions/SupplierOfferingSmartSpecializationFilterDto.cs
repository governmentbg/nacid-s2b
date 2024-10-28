using Sc.Models.Entities.Suppliers.Junctions;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.Suppliers.Junctions
{
    public class SupplierOfferingSmartSpecializationFilterDto : FilterDto<SupplierOfferingSmartSpecialization>
    {
        public string Uic { get; set; }
        public string Name { get; set; }
        public int? DistrictId { get; set; }

        public override IQueryable<SupplierOfferingSmartSpecialization> WhereBuilder(IQueryable<SupplierOfferingSmartSpecialization> query)
        {
            if (!string.IsNullOrWhiteSpace(Uic))
            {
                var uic = Uic.Trim().ToLower();
                query = query.Where(e => e.SupplierOffering.Supplier.Institution.Uic.Trim().ToLower() == uic);
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim().ToLower();
                query = query.Where(e => e.SupplierOffering.Supplier.Institution.Name.Trim().ToLower().Contains(name)
                    || e.SupplierOffering.Supplier.Complex.Name.Trim().ToLower().Contains(name));
            }

            if (DistrictId.HasValue)
            {
                query = query.Where(e => e.SupplierOffering.Supplier.Institution.DistrictId == DistrictId || e.SupplierOffering.Supplier.Complex.DistrictId == DistrictId);
            }

            if (IsActive.HasValue)
            {
                if (IsActive.Value)
                {
                    query = query.Where(e => e.SupplierOffering.IsActive);
                }
                else
                {
                    query = query.Where(e => !e.SupplierOffering.IsActive);
                }
            }

            return query;
        }
    }
}
