namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using PayItForward.Data.Abstraction;
    using Dbmodels = PayItForward.Data.Models;

    public class UsersRepository<T, TKey> : IRepository<T, TKey>
        where T : Dbmodels.User
    {
        public UsersRepository(IPayItForwardDbContext context)
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

        // TESTED
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

        // TODO: take care of deleting Donations before deleting user entity
        // TODO: Catch exception when trying to delete User who has Donations, because of Foreign key restriction
        public void HardDelete(T userTodelete)
        {
            // var donations = this.Context.Donations.Where(s => s.UserId == userTodelete.Id);

            // if (donations != null)
            // {
            //    foreach (var donation in donations.ToList())
            //    {
            //        this.Context.Set<Dbmodels.Donation>().Remove(donation);
            //    }
            // }
            this.Context.Set<T>().Remove(userTodelete);
            this.Context.SaveChanges();
        }

        // TESTED IN StartUp
        public void SoftDelete(T userTodelete)
        {
            this.Update(userTodelete);
        }

        public Task<int> SaveAsync()
        {
            return this.Context.SaveChangesAsync();
        }

        public int Save()
        {
            return this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        private void ChangeEntityState(T entity, EntityState entityState)
        {
            this.Context.ChangeEntityState(entity, entityState);
        }
    }
}
