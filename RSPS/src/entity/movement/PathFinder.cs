using RSPS.src.entity.Mobiles;
using RSPS.src.entity.movement.Locations;
using RSPS.src.entity.movement.Locations.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement
{
    /// <summary>
    /// Handles path finding
    /// </summary>
    public sealed class PathFinder
    {


        /// <summary>
        /// Finds a path for a mobile to a given destination
        /// </summary>
        /// <param name="regionManager">The region manager to determine the regions we're pathfinding in</param>
        /// <param name="mob">The mobile</param>
        /// <param name="destination">The destination position</param>
        /// <param name="nearby">Whether moving nearby is acceptable</param>
        /// <param name="nearX">How close we can move nearby on the X axis</param>
        /// <param name="nearY">How close we can move nearby on the Y axis</param>
        public static void FindPath(RegionManager regionManager, Mobile mob, Position destination, bool nearby = false, int nearX = 1, int nearY = 1)
        {
            FindPath(regionManager, mob, destination.X, destination.Y, nearby, nearX, nearY);
        }

        /// <summary>
        /// Finds a path for a mobile to a given destination
        /// </summary>
        /// <param name="regionManager">The region manager to determine the regions we're pathfinding in</param>
        /// <param name="mob">The mobile</param>
        /// <param name="destX">The destination X coordinate</param>
        /// <param name="destY">The destination Y coordinate</param>
        /// <param name="nearby">Whether moving nearby is acceptable</param>
        /// <param name="nearX">How close we can move nearby on the X axis</param>
        /// <param name="nearY">How close we can move nearby on the Y axis</param>
        public static void FindPath(RegionManager regionManager, Mobile mob, int destX, int destY, bool nearby, int nearX, int nearY)
        {
            try
            {
                if (destX == mob.Position.LocalX && destY == mob.Position.LocalY && !nearby)
                {
                    return;
                }
                int height = mob.Position.Z % 4;
                destX -= 8 * mob.Position.RegionX;
                destY -= 8 * mob.Position.RegionY;

                int[][] via = new int[104][];
                int[][] cost = new int[104][];

                List<int> tileQueueX = new();
                List<int> tileQueueY = new();

                for (int v = 0; v < via.Length; v++)
                {
                    via[v] = new int[104];
                }
                for (int xx = 0; xx < cost.Length; xx++)
                {
                    cost[xx] = new int[104];

                    for (int yy = 0; yy < cost[xx].Length; yy++)
                    {
                        cost[xx][yy] = 99999999;
                    } 
                }
                int curX = mob.Position.LocalX;
                int curY = mob.Position.LocalY;

                if (curX > via.Length - 1 || curY > via[curX].Length - 1)
                {
                    return;
                }
                if (curX < via.Length && curY < via[0].Length)
                {
                    via[curX][curY] = 99;
                }
                if (curX < cost.Length && curY < cost[0].Length)
                {
                    cost[curX][curY] = 0;
                }
                int head = 0;
                int tail = 0;

                tileQueueX.Add(curX);
                tileQueueY.Add(curY);

                bool foundPath = false;
                int pathLength = 4000;

                while (tail != tileQueueX.Count && tileQueueX.Count < pathLength)
                {
                    curX = tileQueueX.ElementAt(tail);
                    curY = tileQueueY.ElementAt(tail);
                    int curAbsX = mob.Position.RegionX * 8 + curX;
                    int curAbsY = mob.Position.RegionY * 8 + curY;

                    if (curX == destX && curY == destY)
                    {
                        foundPath = true;
                        break;
                    }
                    tail = (tail + 1) % pathLength;

                    if (cost.Length < curX || cost[curX].Length < curY)
                    {
                        return;
                    }
                    int thisCost = cost[curX][curY] + 1;
                    Position pos = new(curAbsX, curAbsY, height);

                    if (curY > 0 && via[curX][curY - 1] == 0
                            && !regionManager.IsClipped(pos, DirectionType.South))
                    { //blocked south
                        tileQueueX.Add(curX);
                        tileQueueY.Add(curY - 1);
                        via[curX][curY - 1] = 1;
                        cost[curX][curY - 1] = thisCost;
                    }
                    if (curX > 0 && via[curX - 1][curY] == 0
                            && !regionManager.IsClipped(pos, DirectionType.West))
                    { //Blocked west
                        tileQueueX.Add(curX - 1);
                        tileQueueY.Add(curY);
                        via[curX - 1][curY] = 2;
                        cost[curX - 1][curY] = thisCost;
                    }
                    if (curY < 104 - 1 && via[curX][curY + 1] == 0
                            && !regionManager.IsClipped(pos, DirectionType.North))
                    { //blocked north
                        tileQueueX.Add(curX);
                        tileQueueY.Add(curY + 1);
                        via[curX][curY + 1] = 4;
                        cost[curX][curY + 1] = thisCost;
                    }
                    if (curX < 104 - 1 && via[curX + 1][curY] == 0
                            && !regionManager.IsClipped(pos, DirectionType.East))
                    { //blocked east
                        tileQueueX.Add(curX + 1);
                        tileQueueY.Add(curY);
                        via[curX + 1][curY] = 8;
                        cost[curX + 1][curY] = thisCost;
                    }
                    if (curX > 0 && curY > 0 && via[curX - 1][curY - 1] == 0
                            && !regionManager.IsClipped(pos, DirectionType.SouthWest) //southwest
                            && !regionManager.IsClipped(pos, DirectionType.West) //west
                            && !regionManager.IsClipped(pos, DirectionType.South))
                    { //south
                        tileQueueX.Add(curX - 1);
                        tileQueueY.Add(curY - 1);
                        via[curX - 1][curY - 1] = 3;
                        cost[curX - 1][curY - 1] = thisCost;
                    }
                    if (curX > 0 && curY < 104 - 1 && via[curX - 1][curY + 1] == 0
                            && !regionManager.IsClipped(pos, DirectionType.NorthWest) //northwest
                            && !regionManager.IsClipped(pos, DirectionType.West) //west
                            && !regionManager.IsClipped(pos, DirectionType.North))
                    { //north
                        tileQueueX.Add(curX - 1);
                        tileQueueY.Add(curY + 1);
                        via[curX - 1][curY + 1] = 6;
                        cost[curX - 1][curY + 1] = thisCost;
                    }
                    if (curX < 104 - 1 && curY > 0 && via[curX + 1][curY - 1] == 0
                            && !regionManager.IsClipped(pos, DirectionType.SouthEast) //south east
                            && !regionManager.IsClipped(pos, DirectionType.East) //east
                            && !regionManager.IsClipped(pos, DirectionType.South))
                    { //south
                        tileQueueX.Add(curX + 1);
                        tileQueueY.Add(curY - 1);
                        via[curX + 1][curY - 1] = 9;
                        cost[curX + 1][curY - 1] = thisCost;
                    }
                    if (curX < 104 - 1 && curY < 104 - 1 && via[curX + 1][curY + 1] == 0
                            && !regionManager.IsClipped(pos, DirectionType.NorthEast) //north east
                            && !regionManager.IsClipped(pos, DirectionType.East) //east
                            && !regionManager.IsClipped(pos, DirectionType.North))
                    { //north
                        tileQueueX.Add(curX + 1);
                        tileQueueY.Add(curY + 1);
                        via[curX + 1][curY + 1] = 12;
                        cost[curX + 1][curY + 1] = thisCost;
                    }
                }
                if (!foundPath)
                    if (nearby)
                    {
                        int i_223_ = 1000;
                        int thisCost = 100;
                        int i_225_ = 10;

                        for (int x = destX - i_225_; x <= destX + i_225_; x++)
                            for (int y = destY - i_225_; y <= destY + i_225_; y++)
                                if (x >= 0 && y >= 0 && x < 104 && y < 104
                                && cost[x][y] < 100)
                                {
                                    int i_228_ = 0;
                                    if (x < destX)
                                        i_228_ = destX - x;
                                    else if (x > destX + nearX - 1)
                                        i_228_ = x - (destX + nearX - 1);
                                    int i_229_ = 0;
                                    if (y < destY)
                                        i_229_ = destY - y;
                                    else if (y > destY + nearY - 1)
                                        i_229_ = y - (destY + nearY - 1);
                                    int i_230_ = i_228_ * i_228_ + i_229_
                                            * i_229_;
                                    if (i_230_ < i_223_ || i_230_ == i_223_
                                            && cost[x][y] < thisCost)
                                    {
                                        i_223_ = i_230_;
                                        thisCost = cost[x][y];
                                        curX = x;
                                        curY = y;
                                    }
                                }
                        if (i_223_ == 1000)
                            return;
                    }
                    else
                        return;
                tail = 0;
                tileQueueX.Insert(tail, curX); // TODO arraylist.set() in java, not sure if insert is the exact same behavior
                tileQueueY.Insert(tail++, curY);
                int l5;
                for (int j5 = l5 = via[curX][curY]; curX != mob.Position.LocalX
                        || curY != mob.Position.LocalY; j5 = via[curX][curY])
                {
                    if (j5 != l5)
                    {
                        l5 = j5;
                        tileQueueX.Insert(tail, curX);
                        tileQueueY.Insert(tail++, curY);
                    }
                    if ((j5 & 2) != 0)
                        curX++;
                    else if ((j5 & 8) != 0)
                        curX--;
                    if ((j5 & 1) != 0)
                        curY++;
                    else if ((j5 & 4) != 0)
                        curY--;
                }
                MovementHandler.PrepareMovement(mob);

                int size = tail--;
                int pathX = mob.Position.RegionX * 8 + tileQueueX.ElementAt(tail);
                int pathY = mob.Position.RegionY * 8 + tileQueueY.ElementAt(tail);

                MovementHandler.AddExternalStep(mob, new Position(pathX, pathY, mob.Position.Z));

                for (int i = 1; i < size; i++)
                {
                    tail--;
                    pathX = mob.Position.RegionX * 8 + tileQueueX.ElementAt(tail);
                    pathY = mob.Position.RegionY * 8 + tileQueueY.ElementAt(tail);
                    MovementHandler.AddExternalStep(mob, new Position(pathX, pathY, mob.Position.Z));
                }
                mob.Movement.FinishMovement();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                mob.Movement.FinishMovement();
            }
        }

    }
}
