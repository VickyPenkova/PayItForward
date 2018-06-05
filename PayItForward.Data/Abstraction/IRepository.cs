namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository<T, TKey> : IDisposable
        where T : class
    {
        void Add(T entity);

        IQueryable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(TKey id);

        T GetById(TKey id);

        // TODO: Update chnages in one call to the database
        // void Update(T entity);
        void HardDelete(T entity);

        void SoftDelete(T entity);

        Task<int> SaveAsync();

        int Save();
    }
}
