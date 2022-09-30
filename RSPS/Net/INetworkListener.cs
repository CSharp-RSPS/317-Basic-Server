using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net
{
    /// <summary>
    /// Processes incoming networking data
    /// </summary>
    public interface INetworkListener : IDisposable
    {

        /// <summary>
        /// Starts the network listener
        /// </summary>
        /// <param name="endpoint">The endpoint</param>
        /// <param name="details">The details of the world the listener is for</param>
        /// <returns>Whether successful</returns>
        public bool Start(NetEndpoint endpoint, WorldDetails details);

        /// <summary>
        /// Handles a connection accept callback
        /// </summary>
        /// <param name="result"></param>
        public void AcceptCallback(IAsyncResult result);

    }
}
