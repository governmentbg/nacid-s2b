using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Suppliers;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers
{
    public class SupplierOfferingCounterRepository : RepositoryBase<SupplierOfferingCounter, SupplierOfferingCounterFilterDto>, ISupplierOfferingCounterRepository
    {
        public SupplierOfferingCounterRepository(ScDbContext context)
            : base(context)
        {
        }

        public async Task<SupplierOfferingCounter> GetCounter(CancellationToken cancellationToken)
        {
            return await context.SupplierOfferingCounters.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
