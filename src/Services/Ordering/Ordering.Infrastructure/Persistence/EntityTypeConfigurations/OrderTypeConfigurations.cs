using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class OrderTypeConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(_ => _.Id);
            builder.Ignore(_ => _.LastModifiedBy);
            builder.Ignore(_ => _.LastModifiedDate);
            builder.Ignore(_ => _.CreatedBy);

            builder.HasMany(_ => _.OrderItems)
                .WithOne(_ => _.Order)
                .HasForeignKey(_ => _.OrderId);
        }
    }
}
