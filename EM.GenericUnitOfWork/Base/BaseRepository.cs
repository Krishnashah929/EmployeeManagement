using EM.EFContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        /// <returns></returns>
        public virtual EntityState Add(T entity)
        {
            return dbSet.Add(entity).State;
        }

        /// <summary>
        /// Method for update.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EntityState Update(T entity)
        {
            return dbSet.Update(entity).State;
        }

        /// <summary>
        /// Method for get 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<TKey>(TKey id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Method for get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        public async Task<T> GetAsync<TKey>(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Method for get 
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Get(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        /// <summary>
        /// Method for find by with predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        /// <summary>
        /// Method for findby 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include)
        {
            return FindBy(predicate).Include(include);
        }

        /// <summary>
        /// Method for get all.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        /// <summary>
        /// Method for get all with page , pagecount
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(int page, int pageCount)
        {
            var pageSize = (page - 1) * pageCount;

            return dbSet.Skip(pageSize).Take(pageCount);
        }

        /// <summary>
        /// Method for get all single include
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(string include)
        {
            return dbSet.Include(include);
        }

        /// <summary>
        /// Method for get all users(include)
        /// </summary>
        /// <param name="include"></param>
        /// <param name="include2"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(string include, string include2)
        {
            return dbSet.Include(include).Include(include2);
        }

        /// <summary>
        /// Method for Exists
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }

        /// <summary>
        /// Method for soft delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EntityState SoftDelete(T entity)
        {
            entity.GetType().GetProperty("IsActive")?.SetValue(entity, false);
            return dbSet.Update(entity).State;
        }

        /// <summary>
        /// Method for hard delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EntityState HardDelete(T entity)
        {
            return dbSet.Remove(entity).State;
        }
      
        /// <summary>
        /// RawSql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<T> RawSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
       
    }
}
