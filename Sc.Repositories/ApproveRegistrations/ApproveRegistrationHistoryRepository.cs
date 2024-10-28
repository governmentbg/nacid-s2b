using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.ApproveRegistrations;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ApproveRegistrations;
using Sc.Repositories.Base;

namespace Sc.Repositories.ApproveRegistrations
{
    public class ApproveRegistrationHistoryRepository : RepositoryBase<ApproveRegistrationHistory, ApproveRegistrationHistoryFilterDto>, IApproveRegistrationHistoryRepository
    {
        public ApproveRegistrationHistoryRepository(ScDbContext context)
            : base(context)
        {
        }
        public override Func<IQueryable<ApproveRegistrationHistory>, IQueryable<ApproveRegistrationHistory>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(ar => ar.File)
                    .Include(ar => ar.Supplier.Institution.Root)
                    .Include(ar => ar.Supplier.Complex)
                    .Include(ar => ar.Supplier)
                        .ThenInclude(s => s.SupplierOfferings),
                IncludeType.NavProperties => e => e
                    .Include(ar => ar.File)
                    .Include(ar => ar.Supplier.Institution.Root)
                    .Include(ar => ar.Supplier.Complex)
                    .Include(ar => ar.Supplier)
                        .ThenInclude(s => s.SupplierOfferings),
                IncludeType.Collections => e => e,
                IncludeType.None => e => e,
                _ => e => e,
            };
        }

    }
}
