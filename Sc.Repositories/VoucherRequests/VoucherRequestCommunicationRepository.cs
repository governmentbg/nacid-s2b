﻿using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.VoucherRequests;
using Sc.Repositories.Base;

namespace Sc.Repositories.VoucherRequests
{
    public class VoucherRequestCommunicationRepository : RepositoryBase<VoucherRequestCommunication, VoucherRequestCommunicationFilterDto>, IVoucherRequestCommunicationRepository
    {
        public VoucherRequestCommunicationRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<VoucherRequestCommunication>, IQueryable<VoucherRequestCommunication>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Entity.SupplierOffering.Supplier.Complex)
                    .Include(s => s.Entity.SupplierOffering.Supplier.Institution),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.Entity),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
