using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate;
using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        ISession GetSession();

        void Flush();

        Task FlushAsync();

        /// <summary>
        /// Получение экземпляра сущности Т по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? GetById(int? id);

        /// <summary>
        /// Асинхронное получение экземпляра сущности Т по Id
        /// </summary>
        /// <param name="id">Id сущности</param>
        /// <returns></returns>
        Task<T?> GetByIdAsync(int? id);

        IQueryable<T> Query();

        IQueryable<T> Query(int id);

        Task<T> LoadAsync(int id);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> query);

        Task<bool> ExistsAsync(int id);

        Task EvictAsync(T entity);

        Task EvictAsync(IEnumerable<T> entities);

        Task<T> SaveAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> SaveOrUpdateAsync(T entity);

        Task<T> MergeAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteByIdAsync(int id);
    }
}