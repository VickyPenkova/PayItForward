﻿namespace PayItForward.Data.Abstraction
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public interface IPayItForwardDbContext
    {
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        void Dispose();

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        void ChangeEntityState(object entity, EntityState entityState);
    }
}
