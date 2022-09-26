using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update
{
    public class EntityUpdate<T> where T : Entity
    {

        private static Dictionary<int, IUpdateProtocol<T>> UpdateTunnel = new Dictionary<int, IUpdateProtocol<T>>();

        public void ExecuteUpdate()
        {
            for (int i = 0; i < UpdateTunnel.Count; i++)
            {
                UpdateTunnel[i].Process();
            }
        }



    }
}
