using SGJ.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGJ.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private readonly Dictionary<Items, int> _inventory = new();

        private void Start()
        {
            var values = Enum.GetValues(typeof(Items)).Cast<Items>();
            
            foreach (var item in values)
                _inventory.Add(item, 0);
        }

        public void AddItemOfType(Items item)
        {
            _inventory[item]++;
        }

        public void ConsumeItemOfType(Items item)
        {
            _inventory[item]--;
        }
    }
}