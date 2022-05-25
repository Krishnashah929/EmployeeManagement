using EM.EFContext;
using EM.GenericUnitOfWork.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EM.GenericUnitOfWork.Uow
{/// <summary>
 /// Interface for unit of work repository for crud operations
 /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <returns>
        /// The number of objects in an Added, Modified, or Deleted state.
        /// </returns>
        int Commit();

        /// <summary>
        /// Commits the asynchronous.
        /// </summary>
        /// <returns>
        /// The number of objects in an Added, Modified, or Deleted state asynchronously.
        /// </returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// The Repository.
        /// </returns>
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;
    }
}
