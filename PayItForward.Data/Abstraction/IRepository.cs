namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Data.Abstraction;

    public interface IRepository<T, TKey> : IDisposable
        where T : class
    {
        void Add(T entity);

        IQueryable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(TKey id);

        T GetById(TKey id);

        Task UpdateAsync(T entity);

        Task<int> HardDeleteAsync(T userTodelete);

        void SoftDelete(T userTodelete);

        Task SaveAsync();
    }
}
