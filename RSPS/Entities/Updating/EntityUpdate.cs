using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating
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
