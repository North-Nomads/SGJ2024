using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGJ.Player
{
    public enum Items
    {
        Sonar,
        LifeAnalizer,
        Grenade,
        Dynamite
    }

    public class PlayerInventory : MonoBehaviour
    {
        private Dictionary<Items, int> _inventory = new Dictionary<Items, int>();

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