using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.ReceivedVouchers;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.ReceivedVouchers
{
    public class ReceivedVoucherFilterDto : FilterDto<ReceivedVoucher>
    {
        public DateTime? FromContractDate { get; set; }
        public DateTime? ToContractDate { get; set; }
        public string ContractNumber { get; set; }

        public ReceivedVoucherState? State { get; set; }

        public int? CompanyId { get; set; }

        public int? SupplierId { get; set; }
        public int? OfferingId { get; set; }

        // Server only
        // This is not null only if user is Institution or Complex authorized representatitve
        public int? AuthorizedRepresentativeUserId { get; set; }

        public override IQueryable<ReceivedVoucher> WhereBuilder(IQueryable<ReceivedVoucher> query)
        {
            if (FromContractDate.HasValue)
            {
                query = query.Where(e => e.ContractDate.Date >= FromContractDate.Value.Date);
            }

            if (ToContractDate.HasValue)
            {
                query = query.Where(e => e.ContractDate.Date <= ToContractDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(ContractNumber))
            {
                query = query.Where(e => e.ContractNumber == ContractNumber);
            }

            if (State.HasValue)
            {
                query = query.Where(e => e.State == State);
            }

            if (CompanyId.HasValue)
            {
                query = query.Where(e => e.CompanyId == CompanyId);
            }

            if (SupplierId.HasValue)
            {
                query = query.Where(e => e.SupplierId == SupplierId || e.SecondSupplierId == SupplierId);
            }

            if (OfferingId.HasValue)
            {
                query = query.Where(e => e.OfferingId == OfferingId || e.SecondOfferingId == OfferingId);
            }

            if (AuthorizedRepresentativeUserId.HasValue)
            {
                query = query.Where(e => e.Supplier.Representative.UserId == AuthorizedRepresentativeUserId.Value
                    || e.Offering.SupplierOfferingTeams.Any(s => s.SupplierTeam.UserId == AuthorizedRepresentativeUserId.Value)
                    || e.SecondSupplier.Representative.UserId == AuthorizedRepresentativeUserId.Value
                    || e.SecondOffering.SupplierOfferingTeams.Any(s => s.SupplierTeam.UserId == AuthorizedRepresentativeUserId.Value));
            }

            return query;
        }
    }
}
