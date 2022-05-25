#region Using    
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
#endregion


namespace EM.EFContext
{

    /// <summary>
    /// I Database Context.
    /// </summary>
    public interface IDatabaseContext
    {
        /// <summary>
        /// Sets this instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// set for the specified entity
        /// </returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// number of state entries interacted with database
        /// </returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="bConfirmAllTransactions">if set to <c>true</c> [confirm all transactions].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// number of state entries interacted with database
        /// </returns>
        Task<int> SaveChangesAsync(bool bConfirmAllTransactions, CancellationToken cancellationToken);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>
        /// number of state entries interacted with database
        /// </returns>
        int SaveChanges();

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="bConfirmAllTransactions">if set to <c>true</c> [confirm all transactions].</param>
        /// <returns>
        /// number of state entries interacted with database
        /// </returns>
        int SaveChanges(bool bConfirmAllTransactions);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();
    }
}
