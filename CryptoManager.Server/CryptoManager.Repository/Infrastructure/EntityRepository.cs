using CryptoManager.Repository.DatabaseContext;
using System;
using System.Collections.Generic;
using CryptoManager.Domain.Contracts.Entities;
using CryptoManager.Domain.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CryptoManager.Repository.Infrastructure
{
    public class EntityRepository<T> : IORM<T>, IDisposable where T : class, IEntity
    {
        protected ApplicationIdentityDbContext DataContext;
        protected readonly DbSet<T> DBSet;
        public EntityRepository(ApplicationIdentityDbContext dataContext)
        {
            DataContext = dataContext;
            DataContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            DBSet = DataContext.Set<T>();
        }

        public async Task<T> InsertAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.IsEnabled = true;
            entity.RegistryDate = DateTime.Now;
            await DBSet.AddAsync(entity);
            await DataContext.SaveChangesAsync();
            DataContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            DataContext.Entry(entity).State = EntityState.Modified;
            await DataContext.SaveChangesAsync();
            DataContext.Entry(entity).State = EntityState.Detached;
        }

        public async Task DeleteAsync(T entity)
        {
            entity.IsExcluded = true;
            await UpdateAsync(entity);
        }

        public async Task<T> GetAsync(Guid id, bool loadFull = false)
        {
            DataContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            var entity = await DBSet.FindAsync(id);
            if (loadFull && entity != null)
            {
                var navigations = DataContext.Entry(entity).Navigations;
                foreach (var navigation in navigations)
                {
                    if (navigation is ReferenceEntry)
                    {
                        navigation.Load();
                    }
                }
                DataContext.Entry(entity).State = EntityState.Detached;
            }
            DataContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return entity;
        }

        public async Task<List<T>> GetAllWithoutDisable()
        {
            return await DBSet.AsNoTracking().Where(a => a.IsExcluded == false && a.IsEnabled).ToListAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await DBSet.AsNoTracking().Where(a => a.IsExcluded == false).ToListAsync();
        }

        public IQueryable<T> GetManyWithoutDisable(Expression<Func<T, bool>> where)
        {
            return DBSet.AsNoTracking().Where(a => a.IsExcluded == false && a.IsEnabled).Where(where);
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DBSet.AsNoTracking().Where(a => a.IsExcluded == false).Where(where);
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }
    }
}
