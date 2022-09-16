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

        public static readonly int WorldCycleMs = 600;

        public static readonly string SERVER_NAME = "Wynn's Framework";

        // Size of receive buffer.
        public static readonly int BufferSize = 256;

        public static readonly Position STARTING_LOCATION = new Position(3222, 3222);

    }
}
