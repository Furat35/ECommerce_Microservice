using Authentication.API.Common;
using Authentication.API.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;
using System.Reflection;

namespace Authentication.API.DataAccess.Contexts
{
    public class AuthenticationContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor?.HttpContext?.User.GetActiveUserId() ?? "unknown";
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = userId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = userId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
    }
}
