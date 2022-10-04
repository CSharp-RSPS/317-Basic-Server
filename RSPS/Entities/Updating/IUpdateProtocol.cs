using RSPS.Entities.Mobiles;
using RSPS.Net.GamePackets;

namespace RSPS.Entities.Updating
{
    public interface IUpdateProtocol<E> where E : Mobile
    {

        public void Process(E entity, PacketWriter payload, PacketWriter block);

    }
}
