using Dapper;
using Sc.Models.Dtos.Companies;
using Sc.Models.Dtos.Nomenclatures.Complexes;
using Sc.Models.Dtos.Nomenclatures.Institutions;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.ReceivedVouchers;
using Sc.Models.Enums.Suppliers;
using Sc.Models.FilterDtos.Base;

namespace Sc.Models.FilterDtos.Reports.ReceivedVouchers
{
    public class OfferingContractReportFilterDto : DapperFilterDto<ReceivedVoucher>
    {
        public ReceivedVoucherState? State { get; set; }

        public DateTime? FromContractDate { get; set; }
        public DateTime? ToContractDate { get; set; }

        public SupplierType? SupplierType { get; set; }
        public int? RootInstitutionId { get; set; }
        public InstitutionDto RootInstitution { get; set; }
        public int? InstitutionId { get; set; }
        public InstitutionDto Institution { get; set; }
        public int? ComplexId { get; set; }
        public ComplexDto Complex { get; set; }

        public int? CompanyId { get; set; }
        public CompanyDto Company { get; set; }

        // If user is supplier this is not null
        public List<int> SupplierIds { get; set; } = new List<int>();

        public override void WhereBuilder(SqlBuilder sqlBuilder)
        {
            if (State.HasValue)
            {
                sqlBuilder.Where("rv.state = @State", new { State });
            }

            if (FromContractDate.HasValue)
            {
                sqlBuilder.Where($"rv.contractdate::date >= '{FromContractDate}'::date");
            }

            if (ToContractDate.HasValue)
            {
                sqlBuilder.Where($"rv.contractdate::date <= '{ToContractDate}'::date");
            }

            if (RootInstitutionId.HasValue)
            {
                sqlBuilder.Where("rootinst.id = @RootInstitutionId", new { RootInstitutionId });

                if (InstitutionId.HasValue && InstitutionId != RootInstitutionId)
                {
                    sqlBuilder.Where("supp.institutionid = @InstitutionId", new { InstitutionId });
                }
            }

            if (ComplexId.HasValue)
            {
                sqlBuilder.Where("supp.complexid = @ComplexId", new { ComplexId });
            }

            if (CompanyId.HasValue)
            {
                sqlBuilder.Where("rv.companyId = @CompanyId", new { CompanyId });
            }

            if (SupplierType.HasValue)
            {
                sqlBuilder.Where("supp.type = @SupplierType", new { SupplierType });
            }

            if (SupplierIds.Any())
            {
                sqlBuilder.Where($"supp.id in ({string.Join(",", SupplierIds)})");
            }
        }
    }
}
