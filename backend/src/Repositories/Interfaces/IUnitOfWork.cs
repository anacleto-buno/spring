using BackendApi.Repositories.Interfaces;

namespace BackendApi.Repositories.Interfaces
{
    /// <summary>
    /// Unit of Work interface for managing transactions and coordinating multiple repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Products repository
        /// </summary>
        IProductRepository Products { get; }

        /// <summary>
        /// Saves all changes made in this unit of work
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Saves all changes made in this unit of work with cancellation token
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Begins a database transaction
        /// </summary>
        /// <returns>Task representing the async operation</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        /// <returns>Task representing the async operation</returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        /// <returns>Task representing the async operation</returns>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Gets a generic repository for the specified entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        IRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Checks if there are any unsaved changes
        /// </summary>
        /// <returns>True if there are unsaved changes, false otherwise</returns>
        bool HasUnsavedChanges();
    }
}
