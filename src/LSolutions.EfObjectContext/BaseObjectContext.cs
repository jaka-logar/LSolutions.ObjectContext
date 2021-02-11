using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using LSolutions.EfObjectContext.Infrastructure;
using LSolutions.EntityRepository.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LSolutions.EfObjectContext
{
    /// <summary>
    ///     Base object context implementation
    /// </summary>
    public class BaseObjectContext : DbContext, IDbContext
    {
        #region Ctor

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="options">DbContextOptions</param>
        public BaseObjectContext(DbContextOptions options) : base(options)
        {
            
        }

        #endregion

        #region Utilities

        /// <summary>
        ///     Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Dynamically load all entity and query type configurations
            IEnumerable<Type> typeConfigurations = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes().Where(type =>
                (type.BaseType?.IsGenericType ?? false) &&
                type.BaseType.GetGenericTypeDefinition() == typeof(CustomEntityTypeConfiguration<>)));

            foreach (Type typeConfiguration in typeConfigurations)
            {
                IMappingConfiguration configuration = (IMappingConfiguration) Activator.CreateInstance(typeConfiguration);
                configuration?.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        ///     Modify the input SQL query by adding passed parameters
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>Modified raw SQL query</returns>
        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            // Add parameters to sql
            for (int i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters?[i] is DbParameter parameter))
                    continue;

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

                // Whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        ///     Detach an entity from the context
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            EntityEntry<TEntity> entityEntry = Entry(entity);
            if (entityEntry == null)
                return;

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        #endregion
    }
}
