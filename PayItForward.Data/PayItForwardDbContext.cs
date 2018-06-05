namespace PayItForward.Data
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data.Abstraction;
    using PayItForward.Data.Models;
    using PayItForward.Data.Models.Abstraction;

    public class PayItForwardDbContext : IdentityDbContext<User>, IPayItForwardDbContext
    {
        public PayItForwardDbContext(DbContextOptions<PayItForwardDbContext> options)

        : base(options)
        {
        }

        public PayItForwardDbContext()
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Donation> Donations { get; set; }

        public virtual DbSet<Story> Stories { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfoRules()
        {
            foreach (var entry in this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            foreach (
                var entry in
                    this.ChangeTracker.Entries()
                        .Where(e => e.Entity is IDeletableEntry && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntry)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
            }
        }
    }
}
