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
