using Authentication.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.DataAccess.EntityTypeConfigurations
{
    public class UserTypeConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Surname)
                .IsRequired();

            builder.Property(_ => _.Mail)
               .IsRequired();

            builder.Property(_ => _.Password)
               .IsRequired();

            builder.Property(_ => _.PasswordSalt)
               .IsRequired();

            builder.Property(_ => _.Role)
               .IsRequired();
        }
    }
}
