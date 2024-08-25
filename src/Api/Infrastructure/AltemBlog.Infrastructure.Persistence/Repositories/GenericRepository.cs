using AltenBlog.Api.Application.Interfaces.Repositories;
using AtenBlog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AltemBlog.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;

        protected DbSet<TEntity> _entity => _dbContext.Set<TEntity>();

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #region Insert Methods
        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            await _entity.AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync();
        }
        public virtual int Add(IEnumerable<TEntity> entities)
        {
            _entity.AddRange(entities);
            return _dbContext.SaveChanges();
        }

        public virtual int Add(TEntity entity)
        {
            _entity.Add(entity);
            return _dbContext.SaveChanges();
        }

        #endregion

        #region Update Methods
        public virtual int Update(TEntity entity)
        {
            _entity.Attach(entity);
            _entity.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            _entity.Attach(entity);
            _entity.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Delete Methods
        public virtual int Delete(Guid id)
        {
            var entity = _entity.Find(id);
            if (entity != null)
                return Delete(entity);

            return 0;
        }

        public virtual int Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _entity.Attach(entity);
            }
            _entity.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _entity.Attach(entity);
            }
            _entity.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var entity = _entity.Find(id);
            if (entity != null)
                return await DeleteAsync(entity);

            return 0;
        }

        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(_entity.Where(predicate));
            return _dbContext.SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(_entity.Where(predicate));
            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region AddOrUpdate Methods
        public virtual int AddOrUpdate(TEntity entity)
        {
            //check the entra with the id already tracked
            if (!_entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                _dbContext.Update(entity);

            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> AddOrUpdateAsync(TEntity entity)
        {
            //check the entra with the id already tracked
            //burada önce lokala bakiyorum performans verbessurung
            if (!_entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                _dbContext.Update(entity);

            return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        #endregion

        #region Get Methods
        public virtual IQueryable<TEntity> AsQueryable() => _entity.AsQueryable();
        IQueryable<TEntity> IGenericRepository<TEntity>.Get(Expression<Func<TEntity, bool>> predicate, bool noTracking, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _entity.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _entity.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);


            if (noTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }



        public virtual Task<List<TEntity>> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }


        public virtual Task<List<TEntity>> GetAll(bool noTraking = true)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[]? includes)
        {
            TEntity found = await _entity.FindAsync(id);

            if (found == null)
                return null;

            if (noTracking)
                _dbContext.Entry(found).State = EntityState.Detached;

            foreach (var includ in includes)
                _dbContext.Entry(found).Reference(includ).Load();//leaser loading

            return found;
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entity;

            if (predicate != null)
                query = query.Where(predicate);

            foreach (var includ in includes)
                query = query.Include(includ);


            if (orderBy != null)
                query = orderBy(query);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[]? includes)
        {
            IQueryable<TEntity> query = _entity;

            if (predicate != null)
            {
                query = query.Where(predicate);

                if (includes != null)
                    query = ApplyIncludes(query, includes);

                if (noTracking)
                    query = query.AsNoTracking();
            }
            var entity = await query.SingleOrDefaultAsync();

            return entity;
        }

        #endregion

        #region Bulk Methods
        public virtual async Task BulkAddAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                await Task.CompletedTask;

            await _entity.AddRangeAsync(entities);


            await _dbContext.SaveChangesAsync();
        }

        public virtual Task BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual Task BulkDeleteAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task BulkDeleteByIdAsync(IEnumerable<Guid> ids)
        {
            if (ids != null && !ids.Any())
                await Task.CompletedTask;

            _dbContext.RemoveRange(_entity.Where(i => ids.Contains(i.Id)));
            await _dbContext.SaveChangesAsync();
        }

        public virtual Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SaveChanges Methods
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        #endregion



        private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null)
                foreach (var includItem in includes)
                    query = query.Include(includItem);

            return query;
        }


    }
}
