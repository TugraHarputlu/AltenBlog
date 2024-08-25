﻿using AtenBlog.Api.Domain.Models;
using System.Linq.Expressions;

namespace AltenBlog.Api.Application.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<int> AddAsync(TEntity entity);
    int Add(TEntity entity);
    int Add(IEnumerable<TEntity> entities);
    Task<int> AddAsync(IEnumerable<TEntity> entities);


    Task<int> UpdateAsync(TEntity entity);
    int Update(TEntity entity);


    Task<int> DeleteAsync(TEntity entity);
    int Delete(TEntity entity);
    Task<int> DeleteAsync(Guid id);
    int Delete(Guid id);
    bool DeleteRange(Expression<Func<TEntity, bool>> predicate);
    Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);


    Task<int> AddOrUpdateAsync(TEntity entity);
    int AddOrUpdate(TEntity entity);
    IQueryable<TEntity> AsQueryable();
    Task<List<TEntity>> GetAll(bool noTraking = true);

    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetByIdAsync(Guid id, bool noTracking, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
    Task<List<TEntity>> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
    // Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);

    Task BulkDeleteByIdAsync(IEnumerable<Guid> ids);
    Task BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task BulkDeleteAsync(IEnumerable<TEntity> entities);
    Task BulkUpdateAsync(IEnumerable<TEntity> entities);
    Task BulkAddAsync(IEnumerable<TEntity> entities);

    int SaveChanges();
    Task<int> SaveChangesAsync();
}
