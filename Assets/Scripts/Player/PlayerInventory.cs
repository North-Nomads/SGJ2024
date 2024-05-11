using SGJ.GameItems;
using System.Collections.Generic;

namespace SGJ.Player
{
    public class PlayerInventory 
    {
        private Dictionary<Items, int> _inventory = new();
        private int _medkits;

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

        public int Medkits
        {
            get => _inventory[Items.Medkit];
            set
            {
                if (value < 0)
                    return;

                _inventory[Items.Medkit] = value;
            }
        }
    }
}