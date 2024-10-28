using Sc.Models.Entities.ApproveRegistrations;
using Sc.Models.FilterDtos.ApproveRegistrations;
using Sc.Repositories.Base;

namespace Sc.Repositories.ApproveRegistrations
{
    public interface IApproveRegistrationRepository : IRepositoryBase<ApproveRegistration, ApproveRegistrationFilterDto>
    {
    }
}
