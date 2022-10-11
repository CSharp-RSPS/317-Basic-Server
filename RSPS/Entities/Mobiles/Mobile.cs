using RSPS.Entities.Mobiles.Attributes;
using RSPS.Entities.Mobiles.Npcs;
using RSPS.Entities.movement;
using RSPS.Entities.movement.Locations;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = RSPS.Entities.Mobiles.Attributes.Attribute;

namespace RSPS.Entities.Mobiles
{
    /// <summary>
    /// Represents a mobile entity in the game world
    /// </summary>
    public abstract class Mobile : Entity
    {

        public bool NeedsPlacement = false;

        public bool ResetMovementQueue = false;

        public AttributeMap attributeMap = new AttributeMap();

        public int WorldIndex { get; set; } = -1;

        //public EntityFlags Flags = new();

        /// <summary>
        /// Holds the NPC's nearby the mobile
        /// </summary>
        public List<Npc> LocalNpcs { get; private set; }

        /// <summary>
        /// The last position the mobile was at
        /// </summary>
        public Position? LastPosition { get; set; }

        /// <summary>
        /// The state updating cache for the mobile
        /// </summary>
        public PacketWriter? CachedUpdatePacket { get; private set; }

        /// <summary>
        /// The mobile' movement handler
        /// </summary>
        public MobileMovement Movement { get; private set; }

        /// <summary>
        /// The disabled info in case the mobile is disabled
        /// </summary>
        public Disabled? Disabled { get; set; }

        /// <summary>
        /// Whether the mobile is currently disabled
        /// </summary>
        public bool IsDisabled => Disabled != null && Disabled.TimeDisabled > 0;

        /// <summary>
        /// TODO
        /// </summary>
        public dynamic? WalkToEvent { get; private set; }

        public Position CurrentRegion = new Position(0, 0, 0);

        public bool UpdateRequired { get; set; } = false;

        /// <summary>
        /// Creates a new mobile entity
        /// </summary>
        /// <param name="worldId">The world identifier the mobile is part of</param>
        /// <param name="position">The position of the mobile</param>
        /// <param name="movement">The movement handler of the mobile</param>
        public Mobile(Position position, MobileMovement movement) 
            : base(position)
        {
            LastPosition = position;
            Movement = movement;
            LocalNpcs = new();
            //Console.WriteLine("Trying to run!");
            //attributeMap.Define(AttributeName.RunEnergy, new BooleanAttribute(true));
            //var RunEnergy = attributeMap.GetAttribute(AttributeName.RunEnergy);
            //Console.WriteLine("RunEnergy: " + RunEnergy.GetValue());
        }

        public dynamic SubmitWalkToEvent(dynamic walkToEvent)
        {
            if (WalkToEvent != null)
            {
                WalkToEvent.Cancel();
            }
            WalkToEvent = walkToEvent;
            WalkToEvent.Execute();
            return WalkToEvent;
        }

        public void ResetWalkTo()
        {
            if (WalkToEvent != null)
            {
                WalkToEvent.Cancel();
                WalkToEvent = null;
            }
            Disabled = null;
        }

        public abstract void ResetFlags();

    }
}
