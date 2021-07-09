using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LSolutions.EntityRepository.Data;
using LSolutions.EntityRepository.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LSolutions.EfObjectContext.Infrastructure
{
    /// <summary>
    ///     Represents the Entity Framework repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields

        private readonly IDbContext _context;

        private DbSet<TEntity> _entities;

        #endregion

        #region Ctor

        /// <summary>
        ///     EF Repository
        /// </summary>
        /// <param name="context">IDbContext</param>
        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Utilities

        /// <summary>
        ///     Rollback of entity changes and return full error message
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Error message</returns>
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                List<EntityEntry> entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                        // ignored
                    }
                });
            }

            try
            {
                _context.SaveChanges();
                return exception.ToString();
            }
            catch (Exception ex)
            {
                //if after the rollback of changes the context is still not saving,
                //return the full text of the exception that occurred when saving
                return ex.ToString();
            }
        }

        /// <summary>
        ///     Rollback of entity changes and return full error message
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Error message</returns>
        protected async Task<string> GetFullErrorTextAndRollbackEntityChangesAsync(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                List<EntityEntry> entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                        // ignored
                    }
                });
            }

            try
            {
                await _context.SaveChangesAsync();
                return exception.ToString();
            }
            catch (Exception ex)
            {
                //if after the rollback of changes the context is still not saving,
                //return the full text of the exception that occurred when saving
                return ex.ToString();
            }
        }

        /// <summary>
        ///     Detach all entries from the context to increase performance
        /// </summary>
        protected void DetachAllEntities()
        {
            if (_context is DbContext dbContext)
            {
                List<EntityEntry> entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Detached)
                    .ToList();

                foreach (EntityEntry entry in entries)
                {
                    if (entry.Entity != null)
                    {
                        entry.State = EntityState.Detached;
                    }
                }
            }
        }

        #endregion

        #region Methods

        #region Get

        /// <summary>
        ///     Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(object id)
        {
            return Entities.Find(id);
        }

        /// <summary>
        ///     Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual async ValueTask<TEntity> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }

        #endregion

        #region Insert

        /// <summary>
        ///     Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Add(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        ///     Asynchronously insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
            }
        }

        /// <summary>
        ///     Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.AddRange(entities);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        ///     Asynchronously insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.AddRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
            }
        }

        /// <summary>
        ///     Insert entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void InsertAndDetach(TEntity entity)
        {
            Insert(entity);
            DetachAllEntities();
        }

        /// <summary>
        ///     Asynchronously insert entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task InsertAndDetachAsync(TEntity entity)
        {
            await InsertAsync(entity);
            DetachAllEntities();
        }

        /// <summary>
        ///     Insert entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void InsertAndDetach(IEnumerable<TEntity> entities)
        {
            Insert(entities);
            DetachAllEntities();
        }

        /// <summary>
        ///     Asynchronously insert entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task InsertAndDetachAsync(IEnumerable<TEntity> entities)
        {
            await InsertAsync(entities);
            DetachAllEntities();
        }

        #endregion

        #region Update

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        ///     Asynchronously update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
            }
        }

        /// <summary>
        ///     Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.UpdateRange(entities);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        ///     Asynchronously update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.UpdateRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
            }
        }

        /// <summary>
        ///     Update entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void UpdateAndDetach(TEntity entity)
        {
            Update(entity);
            DetachAllEntities();
        }

        /// <summary>
        ///     Asynchronously update entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateAndDetachAsync(TEntity entity)
        {
            await UpdateAsync(entity);
            DetachAllEntities();
        }

        /// <summary>
        ///     Update entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void UpdateAndDetach(IEnumerable<TEntity> entities)
        {
            Update(entities);
            DetachAllEntities();
        }

        /// <summary>
        ///     Asynchronously update entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task UpdateAndDetachAsync(IEnumerable<TEntity> entities)
        {
            await UpdateAsync(entities);
            DetachAllEntities();
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        ///     Asynchronously delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
            }
        }

        /// <summary>
        ///     Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.RemoveRange(entities);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        ///     Asynchronously delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(await GetFullErrorTextAndRollbackEntityChangesAsync(exception), exception);
            }
        }

        /// <summary>
        ///     Delete entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void DeleteAndDetach(TEntity entity)
        {
            Delete(entity);
            DetachAllEntities();
        }

        /// <summary>
        ///     Asynchronously delete entity and clear change tracker
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task DeleteAndDetachAsync(TEntity entity)
        {
            await DeleteAsync(entity);
            DetachAllEntities();
        }

        /// <summary>
        ///     Delete entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void DeleteAndDetach(IEnumerable<TEntity> entities)
        {
            Delete(entities);
            DetachAllEntities();
        }

        /// <summary>
        ///     Asynchronously delete entities and clear change tracker
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task DeleteAndDetachAsync(IEnumerable<TEntity> entities)
        {
            await DeleteAsync(entities);
            DetachAllEntities();
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table
        {
            get { return Entities; }
        }

        /// <summary>
        ///     Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only
        ///     operations
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking
        {
            get { return Entities.AsNoTracking(); }
        }

        /// <summary>
        ///     Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> Entities
        {
            get { return _entities ?? (_entities = _context.Set<TEntity>()); }
        }

        #endregion
    }
}
