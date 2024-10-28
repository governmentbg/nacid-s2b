using Sc.Models.Entities.Suppliers.Junctions;
using Sc.Models.FilterDtos.Suppliers.Junctions;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers.Junctions
{
    public interface ISoSmartSpecializationRepository : IRepositoryBase<SupplierOfferingSmartSpecialization, SupplierOfferingSmartSpecializationFilterDto>
    {
    }
}
