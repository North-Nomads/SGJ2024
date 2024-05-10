﻿using SGJ.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGJ.Player
{
    public static class PlayerSaveController
    {
        private const string GameConfigPath = "Prefabs/gameConfig";
        private const string PlayerConfigPath = "Prefabs/Player/playerDefaults";

        private static int _minLocationsAmount;
        private static int _maxLocationsAmount;
        private static int _currentRunMissionsAmount;
        private static int _currentMissionIndex;

        private static float _defaultPlayerHealth;
        private static int _defaultPlayerAmmo;

        private static float _savedPlayerHealth;
        private static Dictionary<Items, int> _inventory = new();

        public static int CurrentMissionIndex
        {
            get => _currentMissionIndex;
            set
            {
                if (value <= 0 || value > _currentRunMissionsAmount)
                    throw new Exception($"Trying to set mission index less or equals 0 or higher than total missions. Must be in [1, {_currentRunMissionsAmount}");
                _currentMissionIndex = value;
            }
        }
        public static int CurrentRunMissionsAmount => _currentRunMissionsAmount;
        public static float SavedPlayerHealth => _savedPlayerHealth;
        public static Dictionary<Items, int> InventoryItems => _inventory;
        public static float DefaultPlayerHealth => _defaultPlayerHealth;

        public static bool IsCurrentMissionLastOne => _currentMissionIndex == _currentRunMissionsAmount;

        static PlayerSaveController()
        {
            string[] rawText = ParsePlayerConfig();
            SetupDefaultPlayerValues(rawText);
            Debug.Log($"Set up default values: hp={_defaultPlayerHealth}, ammo={_inventory[Items.Ammo]}");

            rawText = ParseGameConfig();
            SetupDefaultGameValues(rawText);
            Debug.Log($"Game locations in range [{_minLocationsAmount}; {_maxLocationsAmount}");

            static void SetupDefaultGameValues(string[] rawText)
            {
                _minLocationsAmount = int.Parse(rawText[0]);
                _maxLocationsAmount = int.Parse(rawText[1]);
            }

            static string[] ParseGameConfig()
            {
                TextAsset text = Resources.Load(GameConfigPath) as TextAsset;
                var rawText = text.text.Split('\n');
                return rawText;
            }

            static string[] ParsePlayerConfig()
            {
                TextAsset text = Resources.Load(PlayerConfigPath) as TextAsset;
                var rawText = text.text.Split('\n');
                return rawText;
            }

            static void SetupDefaultPlayerValues(string[] rawText)
            {
                _defaultPlayerAmmo = int.Parse(rawText[0]);
                _defaultPlayerHealth = int.Parse(rawText[1]);
                _savedPlayerHealth = _defaultPlayerHealth;
                var itemsTypes = Enum.GetValues(typeof(Items)).Cast<Items>();
                foreach (var item in itemsTypes)
                    _inventory.Add(item, 0);

                _inventory[Items.Ammo] = _defaultPlayerAmmo;
            }
        }

        public static void LaunchNewMission()
        {
            _currentRunMissionsAmount = Random.Range(_minLocationsAmount, _maxLocationsAmount);
            CurrentMissionIndex = 1;
            Debug.Log($"Set up mission length: {_currentRunMissionsAmount}");
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
