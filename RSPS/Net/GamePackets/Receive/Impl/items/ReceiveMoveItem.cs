using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Items;
using RSPS.Game.Items.Containers;
using RSPS.Game.UI;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// This packet is sent when a player moves an item from one slot to another.
    /// </summary>
    [PacketInfo(214, 7)]
    public sealed class ReceiveMoveItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShortAdditionalLittleEndian();
            int insertMode = reader.ReadByte();
            int startingSlot = reader.ReadShortAdditionalLittleEndian();
            int newSlot = reader.ReadShortLittleEndian();

            if (interfaceId < 0 || startingSlot < 0 || newSlot < 0)
            {
                return;
            }
            Debug.WriteLine("Move item [Insert mode: " + insertMode + "]");
            ItemContainer? itemContainer = null;

            switch (interfaceId)
            {
                case Interfaces.Inventory: //Inventory
                case Interfaces.InventoryOverlayBank: //Inventory
                case Interfaces.InventoryOverlayShop:
                case Interfaces.InventoryOverlayTrade:
                    itemContainer = player.Inventory;
                    break;

                case Interfaces.BankItemsOverlay: // Bank
                    itemContainer = player.Bank;
                    break;
            }
            if (itemContainer == null)
            {
                Debug.WriteLine("Move item not supported for interface " + interfaceId);
                return;
            }
            itemContainer.Move(startingSlot, newSlot);
            ItemManager.RefreshInterfaceItems(player, itemContainer.Items, interfaceId);
        }

    }
}
