using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
