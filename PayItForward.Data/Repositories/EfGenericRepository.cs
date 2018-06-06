namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data.Abstraction;
    using PayItForward.Data.Models;

    public class EfGenericRepository<T, TKey> : IRepository<T, TKey>
        where T : BaseModel<TKey>
    {
        public EfGenericRepository(IPayItForwardDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        protected DbSet<T> DbSet { get; set; }

        protected IPayItForwardDbContext Context { get; set; }

        public async Task<T> GetByIdAsync(TKey id)
        {
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

        public IQueryable<T> GetAll()
        {
            return this.DbSet;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.FromResult(this.DbSet.ToList());
        }

        public void SoftDelete(T userTodelete)
        {
            userTodelete.IsDeleted = true;
            userTodelete.DeletedOn = DateTime.Now;
        }

        public void HardDelete(T entity)
        {
            this.Context.Set<T>().Remove(entity);
        }

        public Task<int> SaveAsync()
        {
            return this.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public int Save()
        {
            return this.Context.SaveChanges();
        }

        private void ChangeEntityState(T entity, EntityState entityState)
        {
            this.Context.Entry(entity).State = entityState;
        }
    }
}
