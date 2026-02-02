using Microsoft.EntityFrameworkCore;
using Redarbor.Test.Domain.Entities;

namespace Redarbor.Test.Infrastructure.Persistence.Write
{
    public class RedarborEFDbContext : DbContext
    {
        public RedarborEFDbContext(DbContextOptions<RedarborEFDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RedarborEFDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
