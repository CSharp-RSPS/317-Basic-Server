using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Util.Attributes
{
    /// <summary>
    /// Represents an item container type attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ItemContainerTypeAttribute : Attribute
    {

        /// <summary>
        /// The container interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The interface ID of the inventory overlay when the container interface is opened
        /// </summary>
        public int InventoryOverlayInterfaceId { get; private set; }

        /// <summary>
        /// The base container capacity
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// The capacity for members
        /// </summary>
        public int MemberCapacity { get; private set; }

        /// <summary>
        /// Whether to always stack items in the container
        /// </summary>
        public bool AlwaysStack { get; private set; }


        /// <summary>
        /// Creates a new item container type attribute
        /// </summary>
        /// <param name="interfaceId">The container interface ID</param>
        /// <param name="inventoryOverlayInterfaceId">The interface ID of the inventory overlay when the container interface is opened</param>
        /// <param name="capacity">The base container capacity</param>
        /// <param name="memberCapacity">The capacity for members</param>
        /// <param name="alwaysStack">Whether to always stack items in the container</param>
        public ItemContainerTypeAttribute(int interfaceId, int inventoryOverlayInterfaceId, 
            bool alwaysStack, int capacity, int memberCapacity = 0)
        {
            InterfaceId = interfaceId;
            InventoryOverlayInterfaceId = inventoryOverlayInterfaceId;
            AlwaysStack = alwaysStack;
            Capacity = capacity;
            MemberCapacity = memberCapacity <= 0 ? capacity : memberCapacity;
        }

    }
}
