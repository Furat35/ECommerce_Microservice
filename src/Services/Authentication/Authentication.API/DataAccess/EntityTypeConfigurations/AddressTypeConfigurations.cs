using Authentication.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.DataAccess.EntityTypeConfigurations
{
    public class AddressTypeConfigurations : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.AddressLine)
                .IsRequired();

            builder.Property(_ => _.Country)
               .IsRequired();

            builder.Property(_ => _.State)
               .IsRequired();

            builder.Property(_ => _.ZipCode)
               .IsRequired();

            builder.HasOne(_ => _.User)
               .WithOne(_ => _.Address)
               .HasForeignKey<Address>(_ => _.Id);

            builder.Ignore(_ => _.CreatedBy);
            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.LastModifiedBy);
            builder.Ignore(_ => _.LastModifiedDate);
            builder.Ignore(_ => _.IsDeleted);
        }
    }
}
