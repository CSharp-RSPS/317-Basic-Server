using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet
{
    /// <summary>
    /// The possible packet header types
    /// </summary>
    public enum PacketHeaderType
    {

        Fixed,
        VariableByte,
        VariableShort

    }
}
