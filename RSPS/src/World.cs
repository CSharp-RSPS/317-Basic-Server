using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src
{
    public class World
    {
        /**
         * Holds the players for the world, along with the connections registered to the player
         **/
        public static readonly List<Player> players = new List<Player>();

        /**
         * Holds all the npcs for the world
         **/
        public static readonly List<Npc> npcs = new List<Npc>();


    }
}
