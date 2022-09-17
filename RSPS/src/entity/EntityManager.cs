using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity
{
    public abstract class EntityManager<T> : IEntityManager<T> where T : Entity
    {

        public List<T> Entities = new();


        public virtual int GetIndex(T entity)
        {
            return Entities.IndexOf(entity);
        }

        public virtual T Add(T entity) {
            Entities.Add(entity);
            return entity;
        }

        public virtual void Remove(T entity) {
            Entities.Remove(entity);
        }

        public virtual void Dispose() {
            GC.SuppressFinalize(this);

            Entities.Clear();
        }
    }
}
