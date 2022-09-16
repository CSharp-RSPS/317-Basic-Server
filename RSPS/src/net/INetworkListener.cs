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
        /// The endpoint
        /// </summary>
        public string Endpoint { get; }

        /// <summary>
        /// The port
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Starts network listener
        /// </summary>
        /// <returns>Whether initialization was successful</returns>
        public bool Start();

        /// <summary>
        /// Handles a connection accept callback
        /// </summary>
        /// <param name="result"></param>
        public void AcceptCallback(IAsyncResult result);

        /// <summary>
        /// Handles a connection data read callback
        /// </summary>
        /// <param name="result"></param>
        public void ReadCallback(IAsyncResult result);

    }
}
