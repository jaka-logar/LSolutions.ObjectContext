using System;
using LSolutions.EntityRepository.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LSolutions.EfObjectContext.Infrastructure
{
    /// <summary>
    ///     Represents DB context
    /// </summary>
    public interface IDbContext : IDisposable
    {
        #region Methods

        /// <summary>
        ///     Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        ///     Saves all changes made in this context to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        int SaveChanges();

        /// <summary>
        ///     Detach an entity from the context
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>
        ///     Provides access to database related information and operations for this context.
        /// </summary>
        DatabaseFacade Database { get; }

        #endregion
    }
}