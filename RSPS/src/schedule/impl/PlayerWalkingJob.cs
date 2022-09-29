using RSPS.src.entity.Mobiles.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.schedule.impl
{
    public class PlayerWalkingJob : Job
    {

        private Player player;

        public PlayerWalkingJob(string name, Player player,TimeSpan delay) : base(name, delay)
        {
            this.player = player;
        }

        protected override void Perform()
        {
            Console.WriteLine("Player is walking!");
            
            //player.Movement.ProcessMovements();
        }

    }
}
