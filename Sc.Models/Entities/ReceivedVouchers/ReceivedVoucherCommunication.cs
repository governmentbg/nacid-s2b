using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Notifications;

namespace Sc.Models.Entities.ReceivedVouchers
{
    public class ReceivedVoucherCommunication : BaseCommunication
    {
        [Skip]
        public ReceivedVoucher Entity { get; set; }
    }

    public class ReceivedVoucherCommunicationConfiguration : IEntityTypeConfiguration<ReceivedVoucherCommunication>
    {
        public void Configure(EntityTypeBuilder<ReceivedVoucherCommunication> builder)
        {
            builder.Property(e => e.Text)
                .HasMaxLength(500);
        }
    }
}
