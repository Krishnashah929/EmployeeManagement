#region using
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
#endregion

namespace EM.GenericUnitOfWork.Base
{
    /// <summary>
    /// Interface for generic repository for crud operations
    /// </summary>
    public interface IRepository<T>
    {
        /// <returns>The Entity's state</returns>
        EntityState Add(T entity);
        /// <returns>The Entity's state</returns>
        EntityState Update(T entity);
        /// <returns>Entity</returns>
        T Get<TKey>(TKey id);
        T GetByID(int id);
        /// <returns>Task Entity</returns>
        Task<T> GetAsync<TKey>(TKey id);

        /// <returns>The requested Entity</returns>
        T Get(params object[] keyValues);

        /// <returns>Entity</returns>
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        /// <returns>Queryable</returns>
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include);

        /// <returns>List of entities</returns>
        IQueryable<T> GetAll();

        /// <returns>Queryable</returns>
        IQueryable<T> GetAll(int page, int pageCount);

        /// <returns>List of entities</returns>
        IQueryable<T> GetAll(string include);

        /// <returns>List of entities</returns>
        IQueryable<T> RawSql(string sql, params object[] parameters);

        /// <returns>List of entities</returns>
        IQueryable<T> GetAll(string include, string include2);

        /// <summary>
        /// Soft delete with using IsActive flag for entity
        /// </summary>
        /// <returns>The Entity's state</returns>
        EntityState SoftDelete(T entity);

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <returns>The Entity's state</returns>
        EntityState HardDelete(T entity);

        /// <summary>
        /// Hard delete with where condition
        /// </summary>
        public IQueryable<T> HardDelete();

        /// <summary>
        /// AddRange with where condition
        /// add lsit
        /// </summary>
        /// <param name="entity"></param>
        void AddRange(IEnumerable<T> entity);
        
        /// <summary>
        /// RemoveRange with where condition
        /// remove list 
        /// </summary>
        /// <param name="entity"></param>
        void RemoveRange(IEnumerable<T> entity);

        /// <summary>
        /// UpdateRange with where condition
        /// update list
        /// </summary>
        /// <param name="entity"></param>
        void UpdateRange(IEnumerable<T> entity);

        /// <summary>
        /// Exists
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns> boolen </returns>
        bool Exists(Expression<Func<T, bool>> predicate);
    }
}