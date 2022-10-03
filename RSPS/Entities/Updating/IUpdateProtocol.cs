using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating
{
    public interface IUpdateProtocol<E> where E : Entity
    {

        public void Process(E entity, PacketWriter payload, PacketWriter block);

    }
}
