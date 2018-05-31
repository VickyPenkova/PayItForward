namespace PayItForward.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository<T> : IDisposable
        where T : class
    {
        void Add(T entity);

        IQueryable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(object id);

        Task UpdateAsync(T entity);

        Task HardDeleteAsync(object id);

        Task SaveAsync();

        void ChangeStateToDeleted(T entity);
    }
}
