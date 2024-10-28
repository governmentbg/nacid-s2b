using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Repositories.Base;

namespace Sc.Repositories.ReceivedVouchers
{
    public class ReceivedVoucherCertificateRepository : RepositoryBase<ReceivedVoucherCertificate, ReceivedVoucherCertificateFilterDto>, IReceivedVoucherCertificateRepository
    {
        public ReceivedVoucherCertificateRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<ReceivedVoucherCertificate>, IQueryable<ReceivedVoucherCertificate>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.File),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.File),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
