namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    // Later to be used in Controllers to use the data from PayItForward database
    public class EfGenericRepository<T> : IRepository<T>
        where T : class
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

        public async Task<T> GetByIdAsync(object id)
        {
            // to be tested
            return await this.DbSet.FindAsync(id);
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

        public void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public void Delete(object id)
        {
            var entity = this.GetByIdAsync(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
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
        public async Task DeleteAsync(object id)
        {
            var entity = await this.GetByIdAsync(id);
            this.Context.Set<T>().Remove(entity);
            await this.Context.SaveChangesAsync();
        }

        public Task SaveAsync()
        {
            return this.Context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        private void ChangeEntityState(T entity, EntityState entityState)
        {
            this.Context.Entry(entity).State = entityState;
        }
    }
}
