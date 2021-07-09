using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSolutions.EntityRepository.Domain;

namespace LSolutions.EntityRepository.Data
{
    /// <summary>
    ///     Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Methods

        /// <summary>
        ///     Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        TEntity GetById(object id);

        /// <summary>
        ///     Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        ///     Asynchronously insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        ///     Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Asynchronously insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Insert entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        void InsertAndDetach(TEntity entity);

        /// <summary>
        ///     Asynchronously insert entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        Task InsertAndDetachAsync(TEntity entity);

        /// <summary>
        ///     Insert entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        void InsertAndDetach(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Asynchronously insert entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        Task InsertAndDetachAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        ///     Asynchronously update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        ///     Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Asynchronously update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Update entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        void UpdateAndDetach(TEntity entity);

        /// <summary>
        ///     Asynchronously update entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        Task UpdateAndDetachAsync(TEntity entity);

        /// <summary>
        ///     Update entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        void UpdateAndDetach(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Asynchronously update entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        Task UpdateAndDetachAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        ///     Asynchronously delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        ///     Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Asynchronously delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task DeleteAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Delete entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        void DeleteAndDetach(TEntity entity);

        /// <summary>
        ///     Asynchronously delete entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAndDetachAsync(TEntity entity);

        /// <summary>
        ///     Delete entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        void DeleteAndDetach(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Asynchronously delete entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        Task DeleteAndDetachAsync(IEnumerable<TEntity> entities);

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
