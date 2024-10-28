using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Companies;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.VoucherRequests;

namespace Sc.Models.Entities.VoucherRequests
{
    public class VoucherRequest : EntityVersion
    {
        public string Code { get; set; }

        public VoucherRequestState State { get; set; }

        public DateTime CreateDate { get; set; }

        public int RequestUserId { get; set; }
        public int RequestCompanyId { get; set; }
        [Skip]
        public Company RequestCompany { get; set; }

        public int SupplierOfferingId { get; set; }
        [Skip]
        public SupplierOffering SupplierOffering { get; set; }

        public string DeclineReason { get; set; }
    }

    public class VoucherRequestConfiguration : IEntityTypeConfiguration<VoucherRequest>
    {
        public void Configure(EntityTypeBuilder<VoucherRequest> builder)
        {
            builder.HasIndex(e => new { e.SupplierOfferingId, e.RequestCompanyId })
                .IsUnique();

            builder.Property(e => e.Code)
                .HasMaxLength(5)
                .IsFixedLength()
                .IsRequired(false)
                .HasColumnType("character varying");

            builder.HasIndex(e => e.Code)
                .IsUnique();
        }
    }
}
