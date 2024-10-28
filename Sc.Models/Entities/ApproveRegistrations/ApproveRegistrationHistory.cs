using Sc.Models.Entities.ApproveRegistrations.Base;

namespace Sc.Models.Entities.ApproveRegistrations
{
    public class ApproveRegistrationHistory : BaseApproveRegistration
    {
        public int ApproveRegistrationId { get; set; }
        public ApproveRegistrationHistoryFile File { get; set; }
    }
}
