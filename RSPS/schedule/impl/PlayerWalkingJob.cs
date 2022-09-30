using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Schedule.Impl
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

           // MovementHandler.WalkTo()
            
            //player.Movement.ProcessMovements();
        }

    }
}
