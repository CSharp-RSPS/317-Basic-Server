using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement
{
    public class MovementHandler
    {

        public Queue<MovementPoint> MovementPoints { get; } = new Queue<MovementPoint>();
        private readonly Player Player;
        public bool RunToggled { get; set; } = false; 
        public int Energy { get; private set; } = 10000;
        public int Weight { get; private set; } = 0;//for now it's here
        public bool IsRunPath { get; set; } = false;
        public bool IsRunning { get; private set; } = false;


        public MovementHandler(Player player)
        {
            this.Player = player;
        }

        public void ProcessMovements()
        {
            MovementPoint walkPoint = null;
            MovementPoint runPoint = null;

            if (MovementPoints.Count > 0)
            {
                walkPoint = MovementPoints.Dequeue();
            }
            if (MovementPoints.Count > 0)
            {
                if (RunToggled || IsRunPath)
                {
                    runPoint = MovementPoints.Dequeue();
                }
            }

            IsRunning = RunToggled && runPoint != null ? true : false;

            if (!IsRunning && Energy < 10000)
            {
                ProcessRunEnergyRecovery(Energy);
                PacketHandler.SendPacket(Player, new SendRunEnergy(Energy));
            }

            if (walkPoint != null && walkPoint.Direction != -1)
            {
                Player.Position.MovePosition(Misc.DIRECTION_DELTA_X[walkPoint.Direction], Misc.DIRECTION_DELTA_Y[walkPoint.Direction]);
                Player.PrimaryDirection = walkPoint.Direction;
            }

            if (runPoint != null && runPoint.Direction != -1)
            {
                Player.Position.MovePosition(Misc.DIRECTION_DELTA_X[runPoint.Direction], Misc.DIRECTION_DELTA_Y[runPoint.Direction]);
                Player.SecondaryDirection = runPoint.Direction;
                ProcessRunEnergyDepletion(Energy);
            }

            // Check for region changes
            int deltaX = Player.Position.X - (Player.CurrentRegion.GetRegionX() * 8);
            int deltaY = Player.Position.Y - (Player.CurrentRegion.GetRegionY() * 8);
            if (deltaX < 16 || deltaX >= 88 || deltaY < 16 || deltaY > 88)
            {
                if (!(Player.GetType() == typeof(Npc))) {
                    Player.LoadMapRegion();
                }
            }
        }

        public void Reset()
        {
            IsRunPath = false;
            MovementPoints.Clear();

            // Set the base point as this position
            Position point = Player.Position;
            MovementPoints.Enqueue(new MovementPoint(point.X, point.Y, -1));
        }

        private void ProcessRunEnergyDepletion(int energy)
        {
            if (energy == 0)
            {
                RunToggled = false;
                PacketHandler.SendPacket(Player, new SendConfiguration(173, RunToggled));
                return;
            }
            int EnergyUseDamper = 67 + ((67 * Math.Clamp(Weight, 0, 64)) / 64);
            Energy = Math.Clamp(energy - EnergyUseDamper, 0, 10000);
            PacketHandler.SendPacket(Player, new SendRunEnergy(Energy));
        }

        private void ProcessRunEnergyRecovery(int energy)
        {
            if (energy == 10000)
            {
                return;
            }
            int EnergyRecovery = (0 / 6) + 8;
            Energy = Math.Clamp(energy + EnergyRecovery, 0, 10000);
            PacketHandler.SendPacket(Player, new SendRunEnergy(Energy));
        }

        public void FinishPath()
        {
            MovementPoints.Dequeue();
        }

        public void AddToPath(Position position)
        {
            if (MovementPoints.Count == 0)
            {
                Reset();
            }
            MovementPoint last = MovementPoints.Last();
            int deltaX = position.X - last.X;
            //Console.WriteLine("Delta X: " + deltaX);
            int deltaY = position.Y - last.Y;
            //Console.WriteLine("Delta Y: " + deltaY);
            int max = Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));
            //Console.WriteLine("Max: " + max);
            for (int i = 0; i < max; i++)
            {
                if (deltaX < 0)
                {
                    deltaX++;
                }
                else if (deltaX > 0)
                {
                    deltaX--;
                }
                if (deltaY < 0)
                {
                    deltaY++;
                }
                else if (deltaY > 0)
                {
                    deltaY--;
                }
                AddStep(position.X - deltaX, position.Y - deltaY);
            }
        }

        /**
         * Adds a step.
         * 
         * @param x
         *            the X coordinate
         * @param y
         *            the Y coordinate
         */
        private void AddStep(int x, int y)
        {
            if (MovementPoints.Count >= 100)
            {
                return;
            }
            MovementPoint last = MovementPoints.Last();
            int deltaX = x - last.X;
            //Console.WriteLine("Add Step Delta X: " + deltaX);
            int deltaY = y - last.Y;
            //Console.WriteLine("Add Step Delta Y: " + deltaY);
            int direction = Misc.Direction(deltaX, deltaY);
            //Console.WriteLine("Direction: " + direction);
            if (direction > -1)
            {
                MovementPoints.Enqueue(new MovementPoint(x, y, direction));
            }
        }


    }
}
