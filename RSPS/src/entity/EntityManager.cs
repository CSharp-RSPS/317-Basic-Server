using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity
{
    /// <summary>
    /// Manages entity related operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public abstract class EntityManager<T> : IEntityManager<T> where T : Entity
    {

        /// <summary>
        /// Holds the entities being managed
        /// </summary>
        public List<T> Entities = new();


        /// <summary>
        /// Retrieves the index of an entity within the entities collection
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The result</returns>
        public virtual int GetIndex(T entity)
        {
            return Entities.IndexOf(entity);
        }

        /// <summary>
        /// Retrieves an entity by it's collection index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The result</returns>
        public virtual T? ByIndex(int index)
        {
            return (index < 0 || index >= Entities.Count) ? null : Entities[index];
        }

        /// <summary>
        /// Adds an entity to the entities collection
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The entity</returns>
        public virtual T Add(T entity) {
            Entities.Add(entity);
            return entity;
        }

        /// <summary>
        /// Removes an entity from the entities collection by their collection index
        /// </summary>
        /// <param name="index">The index</param>
        public virtual void Remove(int index)
        {
            T? entity = ByIndex(index);

            if (entity == default)
            {
                return;
            }
            Entities.RemoveAt(index);
        }

        /// <summary>
        /// Removes an entity from the entities collection
        /// </summary>
        /// <param name="entity">The entity</param>
        public virtual void Remove(T entity) {
            Entities.Remove(entity);
        }

        public virtual void Dispose() {
            GC.SuppressFinalize(this);

            Entities.Clear();
        }
    }
}
