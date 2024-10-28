using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.ApproveRegistrations;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ApproveRegistrations;
using Sc.Repositories.Base;

namespace Sc.Repositories.ApproveRegistrations
{
    public class ApproveRegistrationRepository : RepositoryBase<ApproveRegistration, ApproveRegistrationFilterDto>, IApproveRegistrationRepository
    {
        public ApproveRegistrationRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<ApproveRegistration>, IQueryable<ApproveRegistration>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(ar => ar.File)
                    .Include(ar => ar.ApproveRegistrationHistories)
                        .ThenInclude(arh => arh.File)
                    .Include(ar => ar.Supplier.SupplierOfferings)
                    .Include(ar => ar.Supplier.Institution.Root)
                    .Include(ar => ar.Supplier.Complex),
                IncludeType.NavProperties => e => e
                    .Include(ar => ar.File)
                    .Include(ar => ar.Supplier.SupplierOfferings)
                    .Include(ar => ar.Supplier.Institution.Root)
                    .Include(ar => ar.Supplier.Complex),
                IncludeType.Collections => e => e
                    .Include(ar => ar.ApproveRegistrationHistories)
                        .ThenInclude(arh => arh.File),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
 