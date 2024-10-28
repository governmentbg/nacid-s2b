using Sc.Models.Entities.Suppliers;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers
{
    public interface ISupplierRepresentativeRepository : IRepositoryBase<SupplierRepresentative, SupplierRepresentativeFilterDto>
    {
    }
}
