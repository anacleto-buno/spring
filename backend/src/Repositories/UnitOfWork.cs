using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using BackendApi.Models;
using BackendApi.Repositories.Interfaces;

namespace BackendApi.Repositories
{
    /// <summary>
    /// Unit of Work implementation for managing transactions and coordinating multiple repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        // Lazy-loaded repositories
        private IProductRepository? _products;

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = new Dictionary<Type, object>();
        }

        public IProductRepository Products
        {
            get
            {
                return _products ??= new ProductRepository(_context);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Transaction is already started");

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No active transaction to commit");

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No active transaction to rollback");

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            
            if (_repositories.ContainsKey(type))
            {
                return (IRepository<T>)_repositories[type];
            }

            var repository = new BaseRepository<T>(_context);
            _repositories.Add(type, repository);
            return repository;
        }

        public bool HasUnsavedChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
                _repositories.Clear();
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
