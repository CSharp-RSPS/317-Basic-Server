using RSPS.src.Data;
using RSPS.src.entity.npc;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement.Locations.Regions
{
    /// <summary>
    /// Manages regions and region related operations
    /// </summary>
    public static class RegionManager
    {

        /// <summary>
        /// The ID's of the loaded regions
        /// </summary>
        public static int[] RegionIds { get; private set; }

        /// <summary>
        /// The map ground file ID's
        /// </summary>
        public static int[] MapGroundFileIds { get; private set; }

        /// <summary>
        /// The map object file ID's
        /// </summary>
        public static int[] MapObjectFileIds { get; private set; }

        /// <summary>
        /// The loaded regions
        /// </summary>
        public static Region[] Regions { get; private set; }


        static RegionManager()
        {
            try
            {
                byte[] abyte2 = FileUtil.GetBuffer("./data/objects/map_index.dat");
                PacketReader stream2 = new(abyte2);

                //int size = abyte2.length / 6;
                int size = stream2.ReadShort(false);
                RegionIds = new int[size];
                MapGroundFileIds = new int[size];
                MapObjectFileIds = new int[size];
                Regions = new Region[size];

                int regionsLoaded = 0;

                for (int i = 0; i < size; i++)
                {
                    RegionIds[i] = stream2.ReadShort(false);
                    MapGroundFileIds[i] = stream2.ReadShort(false);
                    MapObjectFileIds[i] = stream2.ReadShort(false);
                    regionsLoaded++;
                }
                Debug.WriteLine("Initialized " + regionsLoaded + " regions.");
            }
            catch (IOException ex)
            {
                RegionIds = Array.Empty<int>();
                MapGroundFileIds = Array.Empty<int>();
                MapObjectFileIds = Array.Empty<int>();
                Regions = Array.Empty<Region>();

                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Retrieves the region ID for a given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The region ID</returns>
        public static int GetRegionId(Position position)
        {
            int regionX = position.X >> 3;
            int regionY = position.Y >> 3;
            int regionId = ((regionX / 8) << 8) + (regionY / 8);

            return regionId;
        }

        /// <summary>
        /// Retrieves the region for a given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The region</returns>
        public static Region? GetRegion(Position position)
        {
            int regionId = GetRegionId(position);
            Region? region = null;

            foreach (Region r in Regions)
            {
                if (r != null && r.Id == regionId)
                {
                    region = r;
                    break;
                }
            }
            if (region == null)
            {
                region = LoadRegion(regionId);

                if (region == null)
                {
                    Debug.WriteLine("Failed to load region (" + regionId + ") @ " + position.ToString());
                    return null;
                }
            }
            return region;
        }

        /// <summary>
        /// Retrieves whether a given direction based of a given position is clipped or not.
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="direction">The direction</param>
        /// <returns>The result</returns>
        public static bool IsClipped(Position position, DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.East:
                    return (GetClipping(position.X + 1, position.Y, position.Z) & 0x1280180) != 0;

                case DirectionType.North:
                    return (GetClipping(position.X, position.Y + 1, position.Z) & 0x1280120) != 0;

                case DirectionType.NorthEast:
                    return (GetClipping(position.X + 1, position.Y + 1, position.Z) & 0x12801e0) != 0;

                case DirectionType.NorthWest:
                    return (GetClipping(position.X - 1, position.Y + 1, position.Z) & 0x1280138) != 0;

                case DirectionType.South:
                    return (GetClipping(position.X, position.Y - 1, position.Z) & 0x1280102) != 0;

                case DirectionType.SouthEast:
                    return (GetClipping(position.X + 1, position.Y - 1, position.Z) & 0x1280183) != 0;

                case DirectionType.SouthWest:
                    return (GetClipping(position.X - 1, position.Y - 1, position.Z) & 0x128010e) != 0;

                case DirectionType.West:
                    return (GetClipping(position.X - 1, position.Y, position.Z) & 0x1280108) != 0;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Retrieves the clipping value for given coordinates.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="z">The Z coordinate</param>
        /// <returns>The value</returns>
        public static int GetClipping(int x, int y, int z)
        {
            Position pos = new(x, y, z);
            Region? r = GetRegion(pos);

            if (r == null)
            {
                Debug.WriteLine("No region found for: " + pos);
                return 0;
            }
            if (z > 3)
            {
                pos.Z = z;
            }
            return r.GetClip(pos);
        }

        /// <summary>
        /// Retrieves the clipping value for a given tile based of coordinate and future movement.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="z">The Z coordinate</param>
        /// <param name="targetX">The target X coordinate</param>
        /// <param name="targetY">The target Y coordinate</param>
        /// <returns>The result</returns>
        public static bool GetClipping(int x, int y, int z, int targetX, int targetY)
        {
            if (z > 3)
            {
                z = 0;
            }
            int checkX = (x + targetX);
            int checkY = (y + targetY);

            if (targetX == -1 && targetY == 0)
            {
                return (GetClipping(x, y, z) & 0x1280108) == 0;
            }
            else if (targetX == 1 && targetY == 0)
            {
                return (GetClipping(x, y, z) & 0x1280180) == 0;
            }
            else if (targetX == 0 && targetY == -1)
            {
                return (GetClipping(x, y, z) & 0x1280102) == 0;
            }
            else if (targetX == 0 && targetY == 1)
            {
                return (GetClipping(x, y, z) & 0x1280120) == 0;
            }
            else if (targetX == -1 && targetY == -1)
            {
                return ((GetClipping(x, y, z) & 0x128010e) == 0
                        && (GetClipping(checkX - 1, checkY, z) & 0x1280108) == 0
                        && (GetClipping(checkX - 1, checkY, z) & 0x1280102) == 0);
            }
            else if (targetX == 1 && targetY == -1)
            {
                return ((GetClipping(x, y, z) & 0x1280183) == 0
                        && (GetClipping(checkX + 1, checkY, z) & 0x1280180) == 0
                        && (GetClipping(checkX, checkY - 1, z) & 0x1280102) == 0);
            }
            else if (targetX == -1 && targetY == 1)
            {
                return ((GetClipping(x, y, z) & 0x1280138) == 0
                        && (GetClipping(checkX - 1, checkY, z) & 0x1280108) == 0
                        && (GetClipping(checkX, checkY + 1, z) & 0x1280120) == 0);
            }
            else if (targetX == 1 && targetY == 1)
            {
                return ((GetClipping(x, y, z) & 0x12801e0) == 0
                        && (GetClipping(checkX + 1, checkY, z) & 0x1280180) == 0
                        && (GetClipping(checkX, checkY + 1, z) & 0x1280120) == 0);
            }
            else
            {
                Debug.WriteLine("Failed to get clipping for: " + new Position(x, y, z).ToString()
                        + " -> TargetX: " + targetX + ", TargetY: " + targetY);
                return false;
            }
        }

        /// <summary>
        /// Removes the clipping for a given position
        /// </summary>
        /// <param name="position">The position</param>
        public static void RemoveClip(Position position)
        {
            Region? r = GetRegion(position);

            if (r == null)
            {
                Debug.WriteLine("No region found for " + position.ToString());
                return;
            }
            r.RemoveClip(position);
        }

        /// <summary>
        /// Adds a new clipped tile based of given coordinates.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="z">The Z coordinate</param>
        /// <param name="shift">The shift</param>
        private static void AddClip(int x, int y, int z, int shift)
        {
            AddClip(new Position(x, y, z), shift);
        }

        /// <summary>
        /// Adds a new clipped tile based of a given position.
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="shift">The shift</param>
        private static void AddClip(Position position, int shift)
        {
            Region? r = GetRegion(position);

            if (r == null)
            {
                Debug.WriteLine("Failed to find region for " + position
                    .ToString());
                return;
            }
            r.AddClip(position, shift);
        }

        /// <summary>
        /// Adds clipping for an object
        /// </summary>
        /// <param name="id">The object ID</param>
        /// <param name="position">The position</param>
        /// <param name="type">The object type</param>
        /// <param name="rotation">The object rotation</param>
        public static void AddObjectClipping(int id, Position position, int type, int rotation)
        {
            //TODO: ObjectDefinition def = ObjectManager.getSingleton().getDefinition(id);
            dynamic? def = null;

            if (def == null)
            {
                Debug.WriteLine("No object definition found for: " + id);
                return;
            }
            if (def.getType() == 22 && def.isSolid())
            {
                AddClip(position, 0x200000);
            }
            else if (def.getType() >= 9)
            {
                int xSize = def.getXSize(rotation);
                int ySize = def.getYSize(rotation);

                AddClippingForStaticObject(position, xSize, ySize, def.isSolid());
            }
            else if (def.getType() >= 0 && def.getType() <= 3)
            {
                AddClippingForVariableObject(position, def.getType(), rotation, def.isSolid());
            }
        }

        /// <summary>
        /// Adds clipping for a solid object based on object size and given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="xSize">The size on the X axis</param>
        /// <param name="ySize">The size on the Y axis</param>
        /// <param name="solid">Whether the object is solid</param>
        private static void AddClippingForStaticObject(Position position, int xSize, int ySize, bool solid)
        {
            int clipping = 256;

            if (solid)
            {
                clipping += 0x20000;
            }
            for (int i = position.X; i < position.X + xSize; i++)
            {
                for (int i2 = position.Y; i2 < position.Y + ySize; i2++)
                {
                    AddClip(new Position(i, i2, position.Z), clipping);
                }
            }
        }

        /// <summary>
        /// Adds clipping for a variable object based of a position, type and direction.
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="type">The object type</param>
        /// <param name="rotation">The object rotation</param>
        /// <param name="solid">Whether the object is solid</param>
        private static void AddClippingForVariableObject(Position position, int type, int rotation, bool solid)
        {
            int x = position.X;
            int y = position.Y;
            int z = position.Z;

            if (type == 0)
            {
                if (rotation == 0)
                {
                    AddClip(x, y, z, 128);
                    AddClip(x - 1, y, z, 8);

                    if (solid)
                    {
                        AddClip(x, y, z, 65536);
                        AddClip(x - 1, y, z, 4096);
                    }
                }
                else if (rotation == 1)
                {
                    AddClip(x, y, z, 2);
                    AddClip(x, y + 1, z, 32);

                    if (solid)
                    {
                        AddClip(x, y, z, 1024);
                        AddClip(x, y + 1, z, 16384);
                    }
                }
                else if (rotation == 2)
                {
                    AddClip(x, y, z, 8);
                    AddClip(x + 1, y, z, 128);

                    if (solid)
                    {
                        AddClip(x, y, z, 4096);
                        AddClip(x + 1, y, z, 65536);
                    }
                }
                else if (rotation == 3)
                {
                    AddClip(x, y, z, 32);
                    AddClip(x, y - 1, z, 2);

                    if (solid)
                    {
                        AddClip(x, y, z, 16384);
                        AddClip(x, y - 1, z, 1024);
                    }
                }
            }
            else if (type == 1 || type == 3)
            {
                if (rotation == 0)
                {
                    AddClip(x, y, z, 1);
                    AddClip(x - 1, y, z, 16);

                    if (solid)
                    {
                        AddClip(x, y, z, 512);
                        AddClip(x - 1, y + 1, z, 8192);
                    }
                }
                else if (rotation == 1)
                {
                    AddClip(x, y, z, 4);
                    AddClip(x + 1, y + 1, z, 64);

                    if (solid)
                    {
                        AddClip(x, y, z, 2048);
                        AddClip(x + 1, y + 1, z, 32768);
                    }
                }
                else if (rotation == 2)
                {
                    AddClip(x, y, z, 16);
                    AddClip(x + 1, y - 1, z, 1);

                    if (solid)
                    {
                        AddClip(x, y, z, 8192);
                        AddClip(x + 1, y + 1, z, 512);
                    }
                }
                else if (rotation == 3)
                {
                    AddClip(x, y, z, 64);
                    AddClip(x - 1, y - 1, z, 4);

                    if (solid)
                    {
                        AddClip(x, y, z, 32768);
                        AddClip(x - 1, y - 1, z, 2048);
                    }
                }
            }
            else if (type == 2)
            {
                if (rotation == 0)
                {
                    AddClip(x, y, z, 130);
                    AddClip(x - 1, y, z, 8);
                    AddClip(x, y + 1, z, 32);

                    if (solid)
                    {
                        AddClip(x, y, z, 66560);
                        AddClip(x - 1, y, z, 4096);
                        AddClip(x, y + 1, z, 16384);
                    }
                }
                else if (rotation == 1)
                {
                    AddClip(x, y, z, 10);
                    AddClip(x, y + 1, z, 32);
                    AddClip(x + 1, y, z, 128);

                    if (solid)
                    {
                        AddClip(x, y, z, 5120);
                        AddClip(x, y + 1, z, 16384);
                        AddClip(x + 1, y, z, 65536);
                    }
                }
                else if (rotation == 2)
                {
                    AddClip(x, y, z, 40);
                    AddClip(x + 1, y, z, 128);
                    AddClip(x, y - 1, z, 2);

                    if (solid)
                    {
                        AddClip(x, y, z, 20480);
                        AddClip(x + 1, y, z, 65536);
                        AddClip(x, y - 1, z, 1024);
                    }
                }
                else if (rotation == 3)
                {
                    AddClip(x, y, z, 160);
                    AddClip(x, y - 1, z, 2);
                    AddClip(x - 1, y, z, 8);

                    if (solid)
                    {
                        AddClip(x, y, z, 81920);
                        AddClip(x, y - 1, z, 1024);
                        AddClip(x - 1, y, z, 4096);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the region index for a region ID
        /// </summary>
        /// <param name="regionId">The region ID</param>
        /// <returns>The index</returns>
        public static int GetRegionIndex(int regionId)
        {
            for (int i = 0; i < RegionIds.Length; i++)
            {
                if (RegionIds[i] == regionId)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Loads a region for a given region ID
        /// </summary>
        /// <param name="regionId">The region ID</param>
        /// <returns>The loaded region</returns>
        public static Region? LoadRegion(int regionId)
        {
            int index = GetRegionIndex(regionId);

            if (index == -1)
            {
                Debug.WriteLine("Failed to find index for region: " + regionId);
                return null;
            }
            int x = (regionId >> 8) * 64;
            int y = (regionId & 0xff) * 64;

            Region r = new(regionId, new Position(x, y));
            Regions[index] = r;

            try
            {
                byte[] file1 = FileUtil.GetBuffer("./data/objects/maps/" + MapObjectFileIds[index]);
                byte[] file2 = FileUtil.GetBuffer("./data/objects/maps/" + MapGroundFileIds[index]);

                Debug.WriteLine("F's: " + file1 + ", " + file2);

                LoadMaps(r, new PacketReader(file1), new PacketReader(file2));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine("Failed to load maps for region: " + regionId);
            }
            //TODO: NpcManager.getSingleton().loadRegionalNpcs(r);
            //TODO: GroundItemManager.getSingleton().loadRegionalsGroundItems(r);

            Debug.WriteLine("Loaded region: " + regionId);
            return r;
        }

        private static void LoadMaps(Region region, PacketReader str1, PacketReader str2)
        {

            /*if (region.getPosition().getX() >= 0 && region.getPosition().getX() < 104 
                    && region.getPosition().getY() >= 0 && region.getPosition().getY() < 104) {*/

            int[,,] someArray = new int[4,64,64];

            for (int i = 0; i < 4; i++)
            {
                for (int i2 = 0; i2 < 64; i2++)
                {
                    for (int i3 = 0; i3 < 64; i3++)
                    {
                        while (true)
                        {
                            int v = str2.ReadByte(false);

                            if (v == 0)
                            {
                                break;
                            }
                            else if (v == 1)
                            {
                                str2.ReadByte(false);
                                break;
                            }
                            else if (v <= 49)
                            {
                                str2.ReadByte();
                            }
                            else if (v <= 81)
                            {
                                someArray[i,i2,i3] = v - 49;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int i2 = 0; i2 < 64; i2++)
                {
                    for (int i3 = 0; i3 < 64; i3++)
                    {
                        if ((someArray[i,i2,i3] & 1) == 1)
                        {
                            int height = i;

                            if ((someArray[1,i2,i3] & 2) == 2)
                            {
                                height--;
                            }
                            if (height >= 0 && height <= 3)
                            {
                                AddClip(region.Position.X + i2, region.Position.Y + i3, height, 0x200000); //Non object clipping (water etc)
                            }
                        }
                    }
                }
            }
            int objectId = -1;
            int incr;

            while ((incr = str1.ReadByte(false)) != 0)
            {
                objectId += incr;
                int location = 0;
                int incr2;

                while ((incr2 = str1.ReadByte(false)) != 0)
                {
                    location += incr2 - 1;
                    int localX = (location >> 6 & 0x3f);
                    int localY = (location & 0x3f);
                    int height = location >> 12;
                    int objectData = str1.ReadByte(false);
                    int type = objectData >> 2;
                    int direction = objectData & 0x3;

                    if (localX < 0 || localX >= 64 || localY < 0 || localY >= 64)
                    {
                        continue;
                    }
                    if ((someArray[1,localX,localY] & 2) == 2)
                    {
                        height--;
                    }
                    if (height >= 0 && height <= 3)
                    {
                        AddObjectClipping(objectId, new Position(region.Position.X + localX, region.Position.Y + localY, height), type, direction);
                    }
                }
            }
        }

    }
}
