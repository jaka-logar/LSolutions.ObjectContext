using System.Collections.Generic;
using System.Linq;
using LSolutions.EntityRepository.Domain;

namespace LSolutions.EntityRepository.Data
{
    /// <summary>
    ///     Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial interface IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Methods

        /// <summary>
        ///     Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        ///     Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Insert entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        void InsertAndDetach(TEntity entity);

        /// <summary>
        ///     Insert entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        void InsertAndDetach(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        ///     Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Update entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        void UpdateAndDetach(TEntity entity);

        /// <summary>
        ///     Update entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        void UpdateAndDetach(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        ///     Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Delete entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        void DeleteAndDetach(TEntity entity);

        /// <summary>
        ///     Delete entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        void DeleteAndDetach(IEnumerable<TEntity> entities);

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        ///     Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only
        ///     operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }

        #endregion
    }
}
