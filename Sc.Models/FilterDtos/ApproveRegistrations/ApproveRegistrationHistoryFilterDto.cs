using Sc.Models.Entities.ApproveRegistrations;
using Sc.Models.Enums.State;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.ApproveRegistrations
{
    public class ApproveRegistrationHistoryFilterDto : FilterDto<ApproveRegistrationHistory>
    {
        public int? ApproveRegistrationId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int? AdministratedUserId { get; set; }
        public ApproveRegistrationState? State { get; set; }

        public override IQueryable<ApproveRegistrationHistory> WhereBuilder(IQueryable<ApproveRegistrationHistory> query)
        {
            if (ApproveRegistrationId.HasValue)
            {
                query = query.Where(e => e.ApproveRegistrationId == ApproveRegistrationId);
            }

            if (AdministratedUserId.HasValue)
            {
                query = query.Where(e => e.AdministratedUserId == AdministratedUserId);
            }

            if (CreateDate.HasValue)
            {
                query = query.Where(e => e.CreateDate.Date == CreateDate.Value.Date);
            }

            if (FinishDate.HasValue)
            {
                query = query.Where(e => e.FinishDate.HasValue && e.FinishDate.Value.Date == FinishDate.Value.Date);
            }

            if (State.HasValue)
            {
                query = query.Where(e => e.State == State);
            }

            return base.WhereBuilder(query);
        }
    }
}
