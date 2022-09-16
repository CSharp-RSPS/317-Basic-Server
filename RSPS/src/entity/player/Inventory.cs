using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.player
{
    public class Inventory
    {

        public LinkedList<Object> inventory = new LinkedList<Object>();

        public Inventory()
        {

        }

        public Object GetItemBySlot(int slot)
        {
            return inventory.ElementAt(slot);
        }

        public Object GetItem(Object obj)
        {
            return inventory.Find(obj);
        }

        public void ClearInventory()
        {
            inventory.Clear();
        }

        public bool AddItem(Object obj)
        {
            if (inventory.Count == 28)
            {
                return false;
            }
            inventory.AddLast(obj);
            return true;
        }

        public bool RemoveItem(Object obj)
        {
            if (inventory.Count == 0)
            {
                return false;
            }

            return inventory.Remove(obj);
        }

        //public bool RemoveItemBySlot(Object obj)
        //{
        //    if (inventory.Count == 0)
        //    {
        //        return false;
        //    }

        //    return inventory.Remove()
        //}

        //slot
        //itemid
        //max-size = 28


    }
}
