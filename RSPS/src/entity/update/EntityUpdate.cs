using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update
{
    public class EntityUpdate<T> where T : Entity
    {
        public T? Entity;
        public PacketWriter writer;

        private static Dictionary<int, IUpdateProtocol<T>> UpdateTunnel = new Dictionary<int, IUpdateProtocol<T>>();

        public EntityUpdate(T t, PacketWriter writer) {
            Entity = t;
            this.writer = writer;
        }

        public void ExecuteUpdates()
        {
/*            for (int i = 0; i < UpdateTunnel.Count; i++)
            {
                UpdateTunnel[i].Process(Entity, writer);
            }*/
        }

        public void AddUpdateProcess(UpdateState state, IUpdateProtocol<T> updateProtocol)
        {
            UpdateTunnel.Add((int)state, updateProtocol);
        }

    }
}
