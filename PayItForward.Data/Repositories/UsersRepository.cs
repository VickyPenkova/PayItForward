﻿namespace PayItForward.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data.Abstraction;

    public class UsersRepository<T, TKey> : IRepository<T, TKey>
        where T : PayItForward.Data.Models.User
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

        public async Task<T> GetByIdAsync(TKey id)
        {
            return await this.DbSet.FindAsync(id);
        }

        public T GetById(TKey id)
        {
            return this.DbSet.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return this.DbSet;
        }

        public IQueryable<T> GetAllNotDeletedEntities()
        {
            return this.DbSet.Where(x => x.IsDeleted == false);
        }

        public void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        public void Update(TKey id, T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
        }

        // take care of deleting Donations before deleting user entity
        public void HardDelete(T userTodelete)
        {
            this.Context.Set<T>().Remove(userTodelete);
        }

        public void SoftDelete(T userTodelete)
        {
            userTodelete.IsDeleted = true;
            this.ChangeEntityState(userTodelete, EntityState.Modified);
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
