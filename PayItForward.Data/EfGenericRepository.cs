﻿namespace PayItForward.Data
{
    using System;
    using System.Linq;
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

        public virtual T GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            // to be tested
            var entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            this.Context.Set<entity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual void Delete(T entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public virtual void Delete(object id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public IQueryable<T> GetAll()
        {
            return this.DbSet;
        }
    }
}
