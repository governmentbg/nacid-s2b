using Sc.Models.Entities.Suppliers;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.Suppliers
{
    public class SupplierEquipmentFilterDto : FilterDto<SupplierEquipment>
    {
        public int? SupplierId { get; set; }

        public string Name { get; set; }

        public List<int> ExcludeIds { get; set; } = new List<int>();

        public override IQueryable<SupplierEquipment> WhereBuilder(IQueryable<SupplierEquipment> query)
        {
            if (SupplierId.HasValue)
            {
                query = query.Where(e => e.SupplierId == SupplierId);
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim().ToLower();
                query = query.Where(e => e.Name.Trim().ToLower().Contains(name));
            }

            if (ExcludeIds.Any())
            {
                query = query.Where(e => !ExcludeIds.Contains(e.Id));
            }

            return query;
        }
    }
}
