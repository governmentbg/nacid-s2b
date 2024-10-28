using Sc.Models.Entities.Suppliers;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.Suppliers
{
    public class SupplierFilterDto : FilterDto<Supplier>
    {
        public string Name { get; set; }

        public bool HasSupplierOfferings { get; set; }

        public override IQueryable<Supplier> WhereBuilder(IQueryable<Supplier> query)
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim().ToLower();
                query = query.Where(e => e.Institution.Name.Trim().ToLower().Contains(name) || e.Complex.Name.Trim().ToLower().Contains(name));
            }

            if (HasSupplierOfferings)
            {
                query = query.Where(e => e.SupplierOfferings.Any());
            }

            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                var textFilter = $"{TextFilter.Trim().ToLower()}";
                query = query.Where(e => e.Institution.Name.Trim().ToLower().Contains(textFilter) || e.Complex.Name.Trim().ToLower().Contains(textFilter));
            }

            return query;
        }
    }
}
