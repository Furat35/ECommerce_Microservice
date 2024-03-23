using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class AddressTypeConfigurations : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(_ => _.Id);

            builder.HasOne(_ => _.Order)
                .WithOne(_ => _.Address)
                .HasForeignKey<Address>(_ => _.Id);
        }
    }
}
