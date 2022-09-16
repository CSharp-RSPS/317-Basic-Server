using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity
{
    public interface IEntityManager<T> : IDisposable where T : Entity
    {

       // public List<T> Entities { get; }//Not really since we're following up with abstraction but good habit is to do so though, just don't know what im doing wrong with the generics.

        /// <summary>
        /// Adds a new entity to the manager
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        public T Add(T entity);

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        public void Remove(T entity);

    }
}
