using Sc.Models.Entities.Suppliers;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers
{
    public interface ISupplierOfferingRepository : IRepositoryBase<SupplierOffering, SupplierOfferingFilterDto>
    {
        Task<SupplierOffering> GetByIdAndSupplierId(int id, int supplierId, CancellationToken cancellationToken, Func<IQueryable<SupplierOffering>, IQueryable<SupplierOffering>> includesFunc = null);
    }
}
