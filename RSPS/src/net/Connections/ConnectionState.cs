using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Connections
{
    /// <summary>
    /// Holds the possible connection states
    /// </summary>
    public enum ConnectionState
    {

        Disconnected = 0,
        ConnectionRequest = 1,
        Authenticate = 2,
        Cache = 3,//future
        Authenticated = 4

    }
}
