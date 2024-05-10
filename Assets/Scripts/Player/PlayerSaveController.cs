using SGJ.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGJ.Player
{
    public static class PlayerSaveController
    {
        private static float _defaultPlayerHealth;
        private static int _defaultPlayerAmmo;

        private static float _savedPlayerHealth;
        private static Dictionary<Items, int> _inventory = new();

        public static float SavedPlayerHealth => _savedPlayerHealth;
        public static Dictionary<Items, int> InventoryItems => _inventory;
        public static float DefaultPlayerHealth => _defaultPlayerHealth;

        static PlayerSaveController()
        {
            TextAsset text = Resources.Load("Prefabs/Player/playerDefaults") as TextAsset;
            var rawText = text.text.Split('\n');
            _defaultPlayerAmmo = int.Parse(rawText[0]);
            _defaultPlayerHealth = int.Parse(rawText[1]);
            _savedPlayerHealth = _defaultPlayerHealth;


            var itemsTypes = Enum.GetValues(typeof(Items)).Cast<Items>();
            foreach (var item in itemsTypes)
                _inventory.Add(item, 0);

            _inventory[Items.Ammo] = _defaultPlayerAmmo;
            Debug.Log($"Set up default values: hp={_defaultPlayerHealth}, ammo={_inventory[Items.Ammo]}");
        }

        public static void ResetPlayerProgress()
        {
            _savedPlayerHealth = _defaultPlayerHealth;

            var itemsTypes = Enum.GetValues(typeof(Items)).Cast<Items>();
            foreach (var item in itemsTypes)
                _inventory[item] = 0;

            _inventory[Items.Ammo] = _defaultPlayerAmmo;
            Debug.Log($"Reset player progress. Ammo: {_defaultPlayerAmmo}");
        }

        public static void SavePlayerProgress(float currentHealth, Dictionary<Items, int> inventory)
        {
            Debug.Log($"Saved progress: hp left = {currentHealth}, ammo = {inventory[Items.Ammo]}");
            _inventory = inventory;
            _savedPlayerHealth = currentHealth;
        }
    }
}
