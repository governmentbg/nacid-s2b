using Sc.Models.Dtos.ApproveRegistrations.Base;

namespace Sc.Models.Dtos.ApproveRegistrations.Search
{
    public class ApproveRegistrationHistorySearchDto : BaseApproveRegistrationDto
    {
        public int ApproveRegistrationId { get; set; }
        public ApproveRegistrationHistoryFileDto File { get; set; }
    }
}
