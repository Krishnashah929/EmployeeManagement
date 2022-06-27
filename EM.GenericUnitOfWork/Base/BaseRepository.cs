#region using
using EM.EFContext;
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
    ///generic repository for crud operations
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly IDatabaseContext context;

        /// <summary>
        /// The database set.
        /// </summary>
        private readonly DbSet<T> dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(IDatabaseContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
        /// <summary>
        /// Method for dbset.
        /// </summary>
        public DbSet<T> DbSet
        {
            get
            {
                return context.Set<T>();
            }
        }

        /// <summary>
        /// Method for add.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns> Add entity</returns>
        public virtual EntityState Add(T entity)
        {
            return dbSet.Add(entity).State;
        }

        /// <summary>
        /// Method for update.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns> update entity</returns>
        public virtual EntityState Update(T entity)
        {
            return dbSet.Update(entity).State;
        }

        /// <summary>
        /// Method for get 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns>get data</returns>
        public T Get<TKey>(TKey id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Method for get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> get from id</returns>
        public T GetByID(int id)
        {
            try
            {
                var entity = context.Set<T>();
                return entity.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for getasync.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns> get async data</returns>
        public async Task<T> GetAsync<TKey>(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Method for get 
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns get keyvalue></returns>
        public T Get(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        /// <summary>
        /// Method for find by with predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>find by id </returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        /// <summary>
        /// Method for findby 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns> find by id predicate </returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include)
        {
            return FindBy(predicate).Include(include);
        }

        /// <summary>
        /// Method for get all.
        /// </summary>
        /// <returns> get all users</returns>
        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        /// <summary>
        /// Method for get all with page , pagecount
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageCount"></param>
        /// <returns> get all users with parameter</returns>
        public IQueryable<T> GetAll(int page, int pageCount)
        {
            var pageSize = (page - 1) * pageCount;
            return dbSet.Skip(pageSize).Take(pageCount);
        }

        /// <summary>
        /// Method for get all single include
        /// </summary>
        /// <param name="include"></param>
        /// <returns> get all</returns>
        public IQueryable<T> GetAll(string include)
        {
            return dbSet.Include(include);
        }

        /// <summary>
        /// Method for get all users(include)
        /// </summary>
        /// <param name="include"></param>
        /// <param name="include2"></param>
        /// <returns> get all</returns>
        public IQueryable<T> GetAll(string include, string include2)
        {
            return dbSet.Include(include).Include(include2);
        }

        /// <summary>
        /// Method for Exists
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>exists</returns>
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }

        /// <summary>
        /// Method for soft delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>soft delete from database</returns>
        public virtual EntityState SoftDelete(T entity)
        {
            entity.GetType().GetProperty("IsActive")?.SetValue(entity, false);
            return dbSet.Update(entity).State;
        }

        /// <summary>
        /// Method for hard delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>hard delete from database</returns>
        public virtual EntityState HardDelete(T entity)
        {
            return dbSet.Remove(entity).State;
        }

        /// <summary>
        /// Delete record with where condition
        /// </summary>
        /// <returns>hard delete with particular row</returns>
        public IQueryable<T> HardDelete()
        {
            return dbSet;
        }

        /// <summary>
        /// AddRange with where condition
        /// add lsit
        /// </summary>
        /// <param name="entity"></param>
        public void AddRange(IEnumerable<T> entity)
        {
            this.dbSet.AddRange(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Delete record with where condition
        /// </summary>
        /// <returns>Remove list from database.</returns>
        /// <param name="entity"></param>
        public void RemoveRange(IEnumerable<T> entity)
        {
            this.dbSet.RemoveRange(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// UpdateRange with where condition
        /// Update list
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateRange(IEnumerable<T> entity)
        {
            this.dbSet.UpdateRange(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// RawSql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>raw sql</returns>
        public IQueryable<T> RawSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
