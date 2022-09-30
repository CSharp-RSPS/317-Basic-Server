using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.block
{
    public interface IUpdateMask<T> where T : class
    {

        public void ProcessBlock(T t, PacketWriter writer);

    }
}
