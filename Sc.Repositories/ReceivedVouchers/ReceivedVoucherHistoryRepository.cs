using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Repositories.Base;

namespace Sc.Repositories.ReceivedVouchers
{
    public class ReceivedVoucherHistoryRepository : RepositoryBase<ReceivedVoucherHistory, ReceivedVoucherHistoryFilterDto>, IReceivedVoucherHistoryRepository
    {
        public ReceivedVoucherHistoryRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<ReceivedVoucherHistory>, IQueryable<ReceivedVoucherHistory>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.File)
                    .Include(s => s.Company)
                    .Include(s => s.Supplier.Complex)
                    .Include(s => s.Supplier.Institution)
                    .Include(s => s.Supplier.Representative)
                    .Include(s => s.Offering.Settlement)
                    .Include(s => s.Offering)
                        .ThenInclude(n => n.SmartSpecializations.OrderBy(e => e.Type).ThenBy(e => e.SmartSpecialization.ViewOrder).ThenBy(e => e.SmartSpecialization.Name))
                            .ThenInclude(m => m.SmartSpecialization.Root)
                    .Include(s => s.SecondSupplier.Complex)
                    .Include(s => s.SecondSupplier.Institution)
                    .Include(s => s.SecondSupplier.Representative)
                    .Include(s => s.SecondOffering.Settlement)
                    .Include(s => s.SecondOffering)
                        .ThenInclude(n => n.SmartSpecializations.OrderBy(e => e.Type).ThenBy(e => e.SmartSpecialization.ViewOrder).ThenBy(e => e.SmartSpecialization.Name))
                            .ThenInclude(m => m.SmartSpecialization.Root),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.Company)
                    .Include(s => s.Supplier.Complex)
                    .Include(s => s.Supplier.Institution)
                    .Include(s => s.Offering)
                    .Include(s => s.SecondSupplier.Complex)
                    .Include(s => s.SecondSupplier.Institution)
                    .Include(s => s.SecondOffering),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
