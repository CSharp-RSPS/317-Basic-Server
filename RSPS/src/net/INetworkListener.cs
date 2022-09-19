using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net
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
        /// <returns>Whether successful</returns>
        public bool Start(NetEndpoint endpoint);

        /// <summary>
        /// Handles a connection accept callback
        /// </summary>
        /// <param name="result"></param>
        public void AcceptCallback(IAsyncResult result);

    }
}
