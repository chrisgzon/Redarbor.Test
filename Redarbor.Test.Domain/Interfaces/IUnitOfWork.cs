namespace Redarbor.Test.Domain.Interfaces
{
    /// <summary>
    /// Unit of Work pattern interface for managing transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits all changes to the database
        /// </summary>
        Task<int> CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back all changes
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        Task CommitTransactionAsync();
    }
}
