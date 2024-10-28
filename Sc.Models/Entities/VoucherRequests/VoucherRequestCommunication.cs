using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Notifications;

namespace Sc.Models.Entities.VoucherRequests
{
    public class VoucherRequestCommunication : BaseCommunication
    {
        [SkipUpdate, SkipDelete]
        public VoucherRequest Entity { get; set; }
    }

    public class VoucherRequestCommunicationConfiguration : IEntityTypeConfiguration<VoucherRequestCommunication>
    {
        public void Configure(EntityTypeBuilder<VoucherRequestCommunication> builder)
        {
            builder.Property(e => e.Text)
                .HasMaxLength(500);
        }
    }
}
