namespace PayItForward.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository<T, TKey> : IDisposable
        where T : class
    {
        void Add(T entity);

        IQueryable<T> GetAll();

        IQueryable<T> GetAllNotDeletedEntities();

        Task<T> GetByIdAsync(TKey id);

        T GetById(TKey id);

        // TODO: Update chnages in one call to the database
        void HardDelete(T entity);

        void SoftDelete(T entity);

        Task<int> SaveAsync();

        int Save();
    }
}
