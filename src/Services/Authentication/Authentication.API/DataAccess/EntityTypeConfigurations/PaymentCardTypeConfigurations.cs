using Authentication.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.DataAccess.EntityTypeConfigurations
{
    public class PaymentCardTypeConfigurations : IEntityTypeConfiguration<PaymentCard>
    {
        public void Configure(EntityTypeBuilder<PaymentCard> builder)
        {
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.CardName)
                .IsRequired();

            builder.Property(_ => _.CardNumber)
                .IsRequired();

            builder.Property(_ => _.Expiration)
                .IsRequired();

            builder.Property(_ => _.CVV)
                .IsRequired();

            builder.Property(_ => _.PaymentMethod)
                .IsRequired();

            builder.HasOne(_ => _.User)
            .WithOne(_ => _.PaymentCard)
            .HasForeignKey<PaymentCard>(_ => _.Id);

            builder.Ignore(_ => _.CreatedBy);
            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.LastModifiedBy);
            builder.Ignore(_ => _.LastModifiedDate);
            builder.Ignore(_ => _.IsDeleted);
        }
    }
}
