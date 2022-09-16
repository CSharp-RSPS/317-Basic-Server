using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.task.impl
{
    //This task doesn't run. It's way to slow!
    public class MovementTask : Task
    {
        public MovementTask(string name, TimeSpan sleepTime) : base(name, sleepTime)
        {
        }

        protected override void Execute()
        {
            /*for (int i = 0; i < World.players.Count; i++)
            {
                Player player = World.players[i];
                if (player != null)
                {
                    player.MovementHandler.ProcessMovements();
                }
            }*/
        }
    }
}
