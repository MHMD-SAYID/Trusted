
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Trusted.Core.IRepositories;

namespace Trusted.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().AnyAsync(criteria);
        }
        public async Task<T> GetByIdAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return true;
        }
        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return true;
        }
        public bool Update(T entity)
        {
            _context.Update(entity);
            return true;
        }
        public bool Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }

        public bool DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return true;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.FirstOrDefaultAsync(criteria);
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (take != null && take.HasValue)
                query = query.Take(take.Value);

            if (skip != null && take.HasValue)
                query = query.Skip(skip.Value);

            return await query.ToListAsync();
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (take != null && take.HasValue)
                query = query.Take(take.Value);

            if (skip != null && take.HasValue)
                query = query.Skip(skip.Value);

            return await query.ToListAsync();
        }


        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }
    }
}
