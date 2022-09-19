using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net
{
    /// <summary>
    /// Contains network related utilities
    /// </summary>
    public static class NetUtil
    {


        /// <summary>
        /// Retrieves whether a socket is still connected
        /// </summary>
        /// <param name="socket">The socket</param>
        /// <returns>The result</returns>
        [Obsolete("Seems heavy", true)]
        public static bool IsSocketConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
