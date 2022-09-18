using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.schedule.impl
{
    public class PlayerWalkingTask : Schedule
    {

        private Player player;

        public PlayerWalkingTask(string name, Player player,TimeSpan delay) : base(name, delay)
        {
            this.player = player;
        }

        public override void Execute()
        {
            Console.WriteLine("Player is walking!");
            player.MovementHandler.ProcessMovements();
        }

    }
}
