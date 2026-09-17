using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Trusted.Core.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> criteria);
        Task<T> GetByIdAsync(long id);
        Task<List<T>> GetAllAsync();
        Task<int> CountAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> AddRangeAsync(List<T> entities);
        bool Update(T entity);
        bool Delete(T entity);
        bool DeleteRange(List<T> entities);
        Task<int> CommitAsync();
        Task DisposeAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip, string[] includes = null);

        // Transaction support methods
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackTransactionAsync(IDbContextTransaction transaction);
    }
}
