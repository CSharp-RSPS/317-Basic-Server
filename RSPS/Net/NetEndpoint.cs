using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net
{
    /// <summary>
    /// Represents a network endpoint
    /// </summary>
    public class NetEndpoint
    {

        /// <summary>
        /// The host address
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Retrieves the IP endpoint
        /// </summary>
        public IPEndPoint IPEndpoint => new(IPAddress.Parse(Host), Port);


        /// <summary>
        /// Creates a new network endpoint
        /// </summary>
        /// <param name="host">The host address</param>
        /// <param name="port">The port</param>
        public NetEndpoint(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Host, Port);
        }

    }
}
