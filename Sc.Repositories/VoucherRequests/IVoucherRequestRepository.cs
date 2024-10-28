using Sc.Models.Entities.VoucherRequests;
using Sc.Models.FilterDtos.VoucherRequests;
using Sc.Repositories.Base;

namespace Sc.Repositories.VoucherRequests
{
    public interface IVoucherRequestRepository : IRepositoryBase<VoucherRequest, VoucherRequestFilterDto>
    {
    }
}
