using System.Collections;
using System.Collections.Generic;
using Scenes.Interaction.InteractableObject;
using UnityEngine;

namespace InventorySystem
{
    public class SimpleInventory : MonoBehaviour
    {
        public List<Slot> InventorySlots = new List<Slot>();

        public void PickUpItem(Item item)
        {
            /*
        //Heal
            NanoMedic,
        //Poison
            RushSteel,
        //Other
            Gear,
            RedGear,
            BlueGear,
        //KeyItem
            RedGem,
            BlueGem,
            GreenGem
        */
            AddItemByItemName(item.GetItemName());
        }

        public void UseItem(Item item)
        {
            RemoveItemByItemName(item.GetItemName());
        }

        private void AddItemByItemName(Item.ItemProperties.EItemName itemName)
        {
            var slot = IsSlotItemExist(itemName);
        
            if (slot.IsExist == true)
            {
                InventorySlots[slot.Index].AddItem(1);
            }
            else
            {
                InventorySlots.Add(CreateNewSlot(itemName));
                var newSlot = IsSlotItemExist(itemName);
                InventorySlots[newSlot.Index].AddItem(1);
            }
        }
    
        private void RemoveItemByItemName(Item.ItemProperties.EItemName itemName)
        {
            var slot = IsSlotItemExist(itemName);

            if (slot.IsExist == false)
            {
                InventorySlots.Add(CreateNewSlot(itemName));
                Debug.Log("Never have this item before...");
                return;
            }
            else
            {
                if (InventorySlots[slot.Index].Quantity <= 0)
                {
                    Debug.Log("Seem like you don't have this item.");
                    return;
                }
                else if (InventorySlots[slot.Index].Quantity > 0)
                {
                    Debug.Log("Used '" + InventorySlots[slot.Index].Name + "' 1 ea");
                    InventorySlots[slot.Index].RemoveItem(1);
                }
            }
        }

        private Slot CreateNewSlot(Item.ItemProperties.EItemName itemName)
        {
            var newSlot = new Slot();
            newSlot.Name = itemName;
            newSlot.Quantity = 0;

            return newSlot;
        }

        private SItemExistCheck IsSlotItemExist(Item.ItemProperties.EItemName toCheckItemName)
        {
            var existCheck = new SItemExistCheck();
            existCheck.IsExist = false;

            for (int i = 0; i < InventorySlots.Count; i++)
            {
                if (InventorySlots[i].Name == toCheckItemName)
                {
                    existCheck.Index = i;
                    existCheck.IsExist = true;
                }
            }

            return existCheck;
        }
    
        private struct SItemExistCheck
        {
            public int Index;
            public bool IsExist;
        }
    }

    public class Slot
    {
        public Item.ItemProperties.EItemName Name;
        public int Quantity;

        public void AddItem(int value)
        {
            Quantity += value;
        }

        public void RemoveItem(int value)
        {
            if (Quantity <= 0)
            {
                Quantity = 0;
            }
            Quantity -= value;
        }
    }
}