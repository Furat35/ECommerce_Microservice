using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class OrderItemTypeConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Ignore(_ => _.LastModifiedDate);
            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.CreatedBy);
            builder.Ignore(_ => _.LastModifiedBy);
        }
    }
}
