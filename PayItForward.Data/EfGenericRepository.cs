namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data.Abstraction;
    using PayItForward.Data.Models;

    // Later to be used in Controllers to use the data from PayItForward database
    public class EfGenericRepository<T> : IRepository<T>
        where T : BaseModel
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

        public async Task<T> GetByIdAsync(string id)
        {
            // to be tested
            return await this.DbSet.FindAsync(id);
        }

        public virtual T GetById(string id)
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
    }
}
