using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BackendApi.Models;
using BackendApi.Repositories.Interfaces;

namespace BackendApi.Repositories
{
    /// <summary>
    /// Base repository implementation providing common CRUD operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include related properties
            foreach (var includeProperty in includeProperties.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "")
        {
            if (page <= 0)
                throw new ArgumentException("Page must be greater than 0", nameof(page));
            
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalCount = await query.CountAsync();

            // Include related properties
            foreach (var includeProperty in includeProperties.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public virtual async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            string includeProperties = "")
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            IQueryable<T> query = _dbSet;

            // Include related properties
            foreach (var includeProperty in includeProperties.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.UpdateRange(entities);
        }

        public virtual void Remove(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
        }

        public virtual async Task<bool> RemoveByIdAsync(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            Remove(entity);
            return true;
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.RemoveRange(entities);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return await _dbSet.AnyAsync(filter);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter == null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(filter);
        }
    }
}
