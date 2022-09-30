using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Messaging
{
    /// <summary>
    /// Holds the possible contact types
    /// </summary>
    public enum ContactType
    {

        [ContactLimit(200)]
        Friends,

        [ContactLimit(100)]
        Ignores

    }
}
