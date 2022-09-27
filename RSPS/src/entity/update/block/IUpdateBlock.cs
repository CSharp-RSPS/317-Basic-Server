using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.block
{
    public interface IUpdateBlock<T> where T : class
    {

        public void ProcessBlock(T t, PacketWriter writer);

    }
}
