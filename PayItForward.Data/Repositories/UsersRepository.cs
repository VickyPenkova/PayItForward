namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data.Models;
    using Dbmodels = PayItForward.Data.Models;

    public class UsersRepository<T, TKey> : IRepository<T, TKey>
        where T : Dbmodels.User
    {
        public UsersRepository(PayItForwardDbContext context)
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

        // TESTED IN StartUp
        public async Task<T> GetByIdAsync(TKey id)
        {
            return await this.DbSet.FindAsync(id);
        }

        // TESTED IN StartUp
        public virtual T GetById(TKey id)
        {
            return this.DbSet.Find(id);
        }

        // TESTED IN StartUp
        public void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        // TESTED IN StartUp
        public void Update(T entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        // TESTED IN StartUp
        public IQueryable<T> GetAll()
        {
            return this.DbSet;
        }

        // TESTED IN StartUp
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(this.DbSet.AsEnumerable());
        }

        // TESTED IN StartUp
        public async Task UpdateAsync(T entity)
        {
            this.Context.Set<T>().Update(entity);
            await this.Context.SaveChangesAsync();
        }

        // TESTED IN StartUp, Users can not be hard deleted! Cascade delete is needed
        public async Task<int> HardDeleteAsync(T userTodelete)
        {
            var donations = this.Context.Donations.Where(s => s.UserId == userTodelete.Id);

            if (donations != null)
            {
                foreach (var donation in donations.ToList())
                {
                    this.Context.Set<Dbmodels.Donation>().Remove(donation);
                }
            }

            this.Context.Set<T>().Remove(userTodelete);
            return await this.Context.SaveChangesAsync();
        }

        // TESTED IN StartUp
        public void SoftDelete(T userTodelete)
        {
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

        public void Add(Dbmodels.User entity)
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
