using Sc.Models.Entities.ApproveRegistrations;
using Sc.Models.Enums.State;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.ApproveRegistrations
{
    public class ApproveRegistrationFilterDto : FilterDto<ApproveRegistration>
    {
        public string AuthorizedRepresentativeUsername { get; set; }
        public string AuthorizedRepresentativeFullname { get; set; }
        public int? InstitutionId { get; set; }
        public int? ComplexId { get; set; }
        public int? AdministratedUserId { get; set; }
        public ApproveRegistrationState? State { get; set; }

        public override IQueryable<ApproveRegistration> WhereBuilder(IQueryable<ApproveRegistration> query)
        {
            if (AdministratedUserId.HasValue)
            {
                query = query.Where(e => e.AdministratedUserId == AdministratedUserId);
            }

            if (State.HasValue)
            {
                query = query.Where(e => e.State == State);
            }

            return base.WhereBuilder(query);
        }
    }
}
