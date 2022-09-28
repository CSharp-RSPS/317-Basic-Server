using RSPS.src.entity.Mobiles;
using RSPS.src.entity.movement.Locations;
using RSPS.src.entity.movement.Locations.Regions;
using RSPS.src.entity.player;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement
{
    /// <summary>
    /// Handles movement related operations for mobile entities
    /// </summary>
    public static class MovementHandler
    {

        /// <summary>
        /// The max. run energy level
        /// </summary>
        public static readonly int MaxRunEnergy = 10000;

        /// <summary>
        /// The directions for pedestrian X coordinate movement.
        /// </summary>
        private static readonly int[] DirectionDeltaX = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };

        /// <summary>
        /// The directions for pedestrian Y coordinate movement.
        /// </summary>
        private static readonly int[] DirectionDeltaY = new int[] { 1, 1, 1, 0, 0, -1, -1, -1 };



        /// <summary>
        /// Prepares movement of a given mobile
        /// </summary>
        /// <param name="mob">The mobile</param>
        public static void PrepareMovement(Mobile mob)
        {
            if (mob is Player player)
            {
                PlayerMovement playerMovement = (PlayerMovement)player.Movement;

                playerMovement.RunningQueueEnabled = false;
                PlayerMovement.Reset(player);
            }
            mob.Movement.MovementPoints.Clear();
            mob.Movement.MovementPoints.Enqueue(new MovementPoint(mob.Position.X, mob.Position.Y, DirectionType.None));
        }

        /// <summary>
        /// Handles the cycling movement requests of the entity. 
        /// This method receives prior processing to the main updating procedure 
        /// to ensure that all movement is registered in time to be updated in synchronization with the main procedure.
        /// </summary>
        /// <param name="mob">The mobile</param>
        public static void ProcessMovement(Mobile mob)
        {
            if (mob.Movement.Teleported)
            {
                PrepareMovement(mob);
                return;
            }
            if (mob.LastPosition == null) 
            {
                throw new NullReferenceException(nameof(mob.LastPosition));
            }
            MovementPoint? walkingTile = GetNextTile(mob);
            mob.Movement.WalkingDirection = walkingTile == null ? DirectionType.None : walkingTile.Direction;

            int deltaX = mob.Position.X - mob.LastPosition.RegionX * 8;
            int deltaY = mob.Position.Y - mob.LastPosition.RegionY * 8;

            if (mob is Player player)
            {
                PlayerMovement playerMovement = (PlayerMovement)mob.Movement;

                if (playerMovement.Running)
                {
                    MovementPoint? runningTile = GetNextTile(mob);
                    playerMovement.RunningDirection = runningTile == null ? DirectionType.None : runningTile.Direction;

                    if (runningTile != null)
                    {
                        PlayerMovement.ProcessRunEnergyDepletion(player);
                    }
                } 
                else if (!playerMovement.Running && playerMovement.Energy < MaxRunEnergy)
                {
                    PlayerMovement.ProcessRunEnergyRecovery(player);
                }
                if (deltaX < 16 || deltaX >= 88 || deltaY < 16 || deltaY > 88)
                {
                    PacketHandler.SendPacket(player, PacketDefinition.LoadMapRegion);
                }
            }
        }

        /// <summary>
        /// Returns the next movement point in the designated path.
        /// </summary>
        /// <param name="mob">The mobile</param>
        /// <returns>The next movement point</returns>
        private static MovementPoint? GetNextTile(Mobile mob)
        {
            MovementPoint availableTile = mob.Movement.MovementPoints.Dequeue();

            if (availableTile.Direction == DirectionType.None)
            {
                return null;
            }
            int moveX = DirectionDeltaX[availableTile.Direction.GetDirectionValue()];
            int moveY = DirectionDeltaY[availableTile.Direction.GetDirectionValue()];
            int moveZ = mob.Position.Z;

            mob.Position.MovePosition(moveX, moveY, moveZ);

            return availableTile;
        }

        /// <summary>
        /// The prior procedure before the step taken is internally queued.
        /// </summary>
        /// <param name="mob">The mobile</param>
        /// <param name="position">The position</param>
        public static void AddExternalStep(Mobile mob, Position position)
        {
            if (mob.Movement.MovementPoints.Count == 0)
            {
                PrepareMovement(mob);

                if (mob.Movement.MovementPoints.Count == 0)
                {
                    return;
                }
            }
            MovementPoint lastTile = mob.Movement.MovementPoints.Last();

            int deltaX = position.X - lastTile.X;
            int deltaY = position.Y - lastTile.Y;
            int maximumDelta = Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));

            for (int i = 0; i < maximumDelta; i++)
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
                AddInternalStep(mob, new Position(position.X - deltaX, position.Y - deltaY, position.Z));
            }
        }

        /// <summary>
        /// Adds a step into the internal memory of the movement queue.
        /// </summary>
        /// <param name="mob">The mobile</param>
        /// <param name="position">The position</param>
        private static void AddInternalStep(Mobile mob, Position position)
        {
            if (mob.Movement.MovementPoints.Count >= 50 || mob.Movement.MovementPoints.Count == 0)
            {
                return;
            }
            MovementPoint lastTile = mob.Movement.MovementPoints.Last();

            DirectionType direction = MovementUtil.GetDirection(position.X - lastTile.X, position.Y - lastTile.Y);

            if (direction != DirectionType.None)
            {
                mob.Movement.MovementPoints.Enqueue(new MovementPoint(position.X, position.Y, direction));
            }
        }

        /// <summary>
        /// Steps away from a mobile
        /// </summary>
        /// <param name="mob">The mobile to step away with</param>
        public static void StepAwayRandomly(Mobile mob)
        {
            List<DirectionType> directions = Enum.GetValues(typeof(DirectionType)).Cast<DirectionType>().ToList();
            DirectionType rndDirection = directions[RandomUtil.RandomInt(directions.Count - 1)];

            Position? move = GetStepAway(mob, rndDirection);

            if (move == null)
            {
                return;
            }
            WalkTo(mob, move);
        }

        /// <summary>
        /// Steps away from a mobile
        /// </summary>
        /// <param name="mob">The mobile to step away from</param>
        public static void StepAway(Mobile mob)
        {
            Position? move = GetFirstAvailableStepAway(mob);

            if (move == null)
            {
                return;
            }
            WalkTo(mob, move);
        }

        /// <summary>
        /// Retrieves the first available step away
        /// </summary>
        /// <param name="mob">The mobile</param>
        /// <returns>The step away position</returns>
        public static Position? GetFirstAvailableStepAway(Mobile mob)
        {
            Position? move = GetStepAway(mob, DirectionType.East);

            if (move == null)
            {
                move = GetStepAway(mob, DirectionType.West);

                if (move == null)
                {
                    move = GetStepAway(mob, DirectionType.North);

                    if (move == null)
                    {
                        move = GetStepAway(mob, DirectionType.South);

                        if (move == null)
                        {
                            return null;
                        }
                    }
                }
            }
            return move;
        }

        /// <summary>
        /// Retrieves the best step away position
        /// </summary>
        /// <param name="mob">The mobile stepping away</param>
        /// <param name="direction">The direction</param>
        /// <returns>The step-away position</returns>
        public static Position? GetStepAway(Mobile mob, DirectionType direction)
        {
            if (WorldHandler.World.RegionManager.IsClipped(mob.Position, direction))
            {
                return null;
            }
            int x = mob.Position.X;
            int y = mob.Position.Y;

            if (direction == DirectionType.East)
            {
                x--;
            }
            else if (direction == DirectionType.West)
            {
                x++;
            }
            else if (direction == DirectionType.None)
            {
                y--;
            }
            else if (direction == DirectionType.South)
            {
                y++;
            }
            else
            {
                return null;
            }
            return new Position(x, y, mob.Position.Z);
        }

        /// <summary>
        /// Moves a mobile to a new position
        /// </summary>
        /// <param name="mob">The mobile to move</param>
        /// <param name="position">The new position</param>
        public static void Move(Mobile mob, Position position)
        {
            PrepareMovement(mob);

            mob.Position.SetNewPosition(position);
            mob.Movement.Teleported = true;

            if (mob is Player)
            {
                ((PlayerMovement)mob.Movement).MapRegionChanged = true;
            }
        }

        /// <summary>
        /// Walks a mobile to a specified position
        /// </summary>
        /// <param name="mob">The mobile</param>
        /// <param name="position">The position</param>
        public static void WalkTo(Mobile mob, Position position)
        {
            PathFinder.FindPath(WorldHandler.World.RegionManager, mob, position);
        }

        /// <summary>
        /// Walks a mobile nearby a specified position
        /// </summary>
        /// <param name="mob">The mobile</param>
        /// <param name="position">The position</param>
        /// <param name="distance">The nearby distance</param>
        public static void WalkNearby(Mobile mob, Position position, int distance = 1)
        {
            PathFinder.FindPath(WorldHandler.World.RegionManager, mob, position, true, distance, distance);
        }

        /// <summary>
        /// Forces a mobile to walk to a given position
        /// </summary>
        /// <param name="mob">The mobile</param>
        /// <param name="position">The position</param>
        public static void ForceWalk(Mobile mob, Position position)
        {
            PrepareMovement(mob);
            AddExternalStep(mob, position);
            mob.Movement.FinishMovement();
        }

    }
}
