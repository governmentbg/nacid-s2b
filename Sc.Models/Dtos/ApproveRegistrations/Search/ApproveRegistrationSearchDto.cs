using Sc.Models.Dtos.ApproveRegistrations.Base;

namespace Sc.Models.Dtos.ApproveRegistrations.Search
{
    public class ApproveRegistrationSearchDto : BaseApproveRegistrationDto
    {
        public ApproveRegistrationFileDto File { get; set; }

        public List<ApproveRegistrationHistorySearchDto> ApproveRegistrationHistories { get; set; } = new List<ApproveRegistrationHistorySearchDto>();
    }
}
