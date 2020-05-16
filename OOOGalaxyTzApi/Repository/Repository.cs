using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using OOOGalaxyTzApi.Interfaces;
using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApiRepository
{
    public class Repository<T> : IDisposable,
                                 IRepository<T> where T : BaseModel
    {
        private readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        public void Flush()
        {
            try
            {
                if (_session.Transaction?.IsActive == true)
                    _session.Transaction.Commit();
            }
            catch
            {
                if (_session.Transaction?.IsActive == true)
                    _session.Transaction.Rollback();

                throw;
            }
        }

        public async Task FlushAsync()
        {
            try
            {
                if (_session.Transaction?.IsActive == true)
                    await _session.Transaction.CommitAsync();
            }
            catch
            {
                if (_session.Transaction?.IsActive == true)
                    await _session.Transaction.RollbackAsync();

                throw;
            }
        }

        public T? GetById(int? id)
        {
            return id is null || id == 0 ? null : _session.Get<T>(id);
        }

        public async Task<T?> GetByIdAsync(int? id)
        {
            return id is null || id == 0 ? null : await _session.GetAsync<T>(id);
        }

        public IQueryable<T> Query()
        {
            return _session.Query<T>();
        }

        public IQueryable<T> Query(int id)
        {
            return _session.Query<T>()
                           .Where(i => i.Id == id);
        }

        private bool Exists(Expression<Func<T, bool>> query)
        {
            return Query().Any(query);
        }

        public bool Exists(int id)
        {
            return Exists(q => q.Id == id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> query)
        {
            return await Query()
                      .CountAsync(query) > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await ExistsAsync(q => q.Id == id);
        }

        public T Save(T entity)
        {
            _session.Save(entity);
            return entity;
        }

        public async Task<T> SaveAsync(T entity)
        {
            await _session.SaveAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _session.Update(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await _session.UpdateAsync(entity);
            return entity;
        }

        public T SaveOrUpdate(T entity)
        {
            _session.SaveOrUpdate(entity);
            return entity;
        }

        public async Task<T> SaveOrUpdateAsync(T entity)
        {
            await _session.SaveOrUpdateAsync(entity);
            return entity;
        }

        public T Merge(T entity)
        {
            _session.Merge(entity);
            return entity;
        }

        public async Task<T> MergeAsync(T entity)
        {
            await _session.MergeAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _session.Delete(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await _session.DeleteAsync(entity);
        }

        public void DeleteById(int id)
        {
            Delete(_session.Load<T>(id));
        }

        public async Task DeleteByIdAsync(int id)
        {
            await DeleteAsync(await _session.LoadAsync<T>(id));
        }

        public async Task<T> LoadAsync(int id) => await _session.LoadAsync<T>(id);

        public async Task EvictAsync(T entity) => await _session.EvictAsync(entity);

        public async Task EvictAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                await _session.EvictAsync(entity);
        }

        public ISession GetSession() => _session;

        public void Dispose()
        {
        }
    }
}