using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Connections
{
    public enum ConnectionState
    {

        None = 0,
        Handshake = 1,
        Authenticate = 2,
        Cache = 3,//future
        Authenticated = 4

    }
}
