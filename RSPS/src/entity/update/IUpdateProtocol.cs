using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update
{
    public interface IUpdateProtocol<E> where E : Entity
    {

        public void Process(E entity, PacketWriter writer, PacketWriter stateBlock);

    }
}
