﻿using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Repositories.Base;

namespace Sc.Repositories.ReceivedVouchers
{
    public interface IReceivedVoucherRepository : IRepositoryBase<ReceivedVoucher, ReceivedVoucherFilterDto>
    {
    }
}
