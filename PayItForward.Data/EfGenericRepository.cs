namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data.Abstraction;
    using PayItForward.Data.Models;
    using PayItForward.Data.Models.Abstraction;

    // Later to be used in Controllers to use the data from PayItForward database
    public class EfGenericRepository<T, TKey> : IRepository<T, TKey>
        where T : BaseModel<TKey>
    {
        public EfGenericRepository(PayItForwardDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        protected DbSet<T> DbSet { get; set; }

        protected PayItForwardDbContext Context { get; set; }

        public async Task<T> GetByIdAsync(TKey id)
        {
            // to be tested
            return await this.DbSet.FindAsync(id);
        }

        public virtual T GetById(TKey id)
        {
            return this.DbSet.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        public void Update(T entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public IQueryable<T> GetAll()
        {
            return this.DbSet;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(this.DbSet.AsEnumerable());
        }

        // to be tested
        public async Task UpdateAsync(T entity)
        {
            this.Context.Set<T>().Update(entity);
            await this.Context.SaveChangesAsync();
        }

        // to be tested
        public void HardDelete(T userTodelete)
        {
            this.Context.Set<T>().Remove(userTodelete);
        }

        public void SoftDelete(T userTodelete)
        {
            userTodelete.IsDeleted = true;
            userTodelete.DeletedOn = DateTime.Now;
            this.Update(userTodelete);
        }

        public Task SaveAsync()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();
            return this.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public Task<int> HardDeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        private void ChangeEntityState(T entity, EntityState entityState)
        {
            // this.Context.Entry(entity).State = entityState;
            var entry = this.Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = entityState;
        }

        private void ApplyAuditInfoRules()
        {
            foreach (var entry in this.Context.ChangeTracker.Entries()
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
                    this.Context.ChangeTracker.Entries()
                        .Where(e => e.Entity is IDeletableEntry && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntry)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}
