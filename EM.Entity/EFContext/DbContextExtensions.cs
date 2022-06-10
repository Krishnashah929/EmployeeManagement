#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Threading.Tasks;
#endregion

namespace EM.EFContext
{
    /// <summary>
    /// dbcontext extension class
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Static class of sql query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<T[]> SqlQuery<T>(this DbContext db, string sql, params object[] parameters) where T : class
        {
            using (var db2 = new ContextForQueryType<T>(db.Database.GetDbConnection(), db.Database.CurrentTransaction))
            {
                return await db2.Set<T>().FromSqlRaw(sql, parameters).ToArrayAsync();
            }
        }

        /// <summary>
        /// private class for context for query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ContextForQueryType<T> : DbContext where T : class
        {
            private readonly DbConnection connection;
            private readonly IDbContextTransaction transaction;

            public ContextForQueryType(DbConnection connection, IDbContextTransaction tran)
            {
                this.connection = connection;
                this.transaction = tran;

                if (tran != null)
                {
                    this.Database.UseTransaction((tran as IInfrastructure<DbTransaction>).Instance);
                }
            }
            
            /// <summary>
            /// override method of on config
            /// </summary>
            /// <param name="optionsBuilder"></param>
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (transaction != null)
                {
                    optionsBuilder.UseSqlServer(connection);
                }
                else
                {
                    optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());
                }
            }

            /// <summary>
            /// override method of model creating.
            /// </summary>
            /// <param name="modelBuilder"></param>
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>().HasNoKey().ToView(null);
            }
        }
    }

    /// <summary>
    /// public class of output parameter
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class OutputParameter<TValue>
    {
        private bool _hasOperationFinished = false;

        public TValue _value;
        public TValue Value
        {
            get
            {
                if (!_hasOperationFinished)
                    throw new InvalidOperationException("Operation has not finished.");

                return _value;
            }
        }

        /// <summary>
        /// internal method for set value
        /// </summary>
        /// <param name="value"></param>
        internal void SetValueInternal(object value)
        {
            _hasOperationFinished = true;
            _value = (TValue)value;
        }
    }
}
