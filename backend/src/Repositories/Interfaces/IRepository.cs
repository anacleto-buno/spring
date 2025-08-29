using System.Linq.Expressions;

namespace BackendApi.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface providing common CRUD operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its identifier
        /// </summary>
        /// <param name="id">The entity identifier</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<T?> GetByIdAsync(object id);

        /// <summary>
        /// Gets all entities matching the optional filter
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <param name="orderBy">Optional ordering function</param>
        /// <param name="includeProperties">Comma-delimited list of related properties to include</param>
        /// <returns>Collection of entities</returns>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Gets a paged result of entities
        /// </summary>
        /// <param name="page">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="filter">Optional filter expression</param>
        /// <param name="orderBy">Optional ordering function</param>
        /// <param name="includeProperties">Comma-delimited list of related properties to include</param>
        /// <returns>Paged result containing entities and metadata</returns>
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Gets the first entity matching the filter
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <param name="includeProperties">Comma-delimited list of related properties to include</param>
        /// <returns>The first matching entity or null</returns>
        Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            string includeProperties = "");

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Adds multiple entities
        /// </summary>
        /// <param name="entities">The entities to add</param>
        /// <returns>Task representing the async operation</returns>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Updates multiple entities
        /// </summary>
        /// <param name="entities">The entities to update</param>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        void Remove(T entity);

        /// <summary>
        /// Removes an entity by its identifier
        /// </summary>
        /// <param name="id">The entity identifier</param>
        /// <returns>True if the entity was removed, false if not found</returns>
        Task<bool> RemoveByIdAsync(object id);

        /// <summary>
        /// Removes multiple entities
        /// </summary>
        /// <param name="entities">The entities to remove</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Checks if any entity matches the filter
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <returns>True if any entity matches, false otherwise</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Counts entities matching the filter
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <returns>Count of matching entities</returns>
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    }
}
