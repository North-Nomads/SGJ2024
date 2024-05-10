using SGJ.GameItems;
using System.Collections.Generic;

namespace SGJ.Player
{
    public class PlayerInventory 
    {
        private Dictionary<Items, int> _inventory = new();

        public PlayerInventory() => _inventory = PlayerSaveController.InventoryItems;

        public int Ammo
        {
            get => _inventory[Items.Ammo];
            set
            {
                if (value < 0)
                    return;

                _inventory[Items.Ammo] = value;
            }
        }

        public Dictionary<Items, int> Inventory => _inventory;

        public void ConsumeItemOfType(Items item, int quantity = 1) => _inventory[item] -= quantity;
    }
}