using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class PaymentCardTypeConfigurations : IEntityTypeConfiguration<PaymentCard>
    {
        public void Configure(EntityTypeBuilder<PaymentCard> builder)
        {
            builder.HasKey(_ => _.Id);

            builder.HasOne(_ => _.Order)
                .WithOne(_ => _.PaymentCard)
                .HasForeignKey<PaymentCard>(_ => _.Id);
        }
    }
}
