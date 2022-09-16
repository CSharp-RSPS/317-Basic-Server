using RSPS.src.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src
{
    public static class Constants
    {

        public static readonly string SERVER_NAME = "Wynn's Framework";
        public static readonly string ENDPOINT = "0.0.0.0";
        //public static readonly int PORT = 43595;
        public static readonly int PORT = 43594;

        // Size of receive buffer.
        public static readonly int BufferSize = 256;

        public static readonly Position STARTING_LOCATION = new Position(3222, 3222);

    }
}
