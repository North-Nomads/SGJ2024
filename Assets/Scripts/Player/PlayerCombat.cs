using SGJ.Combat;
using SGJ.GameItems;
using SGJ.Proprs;
using System;
using UnityEngine;

namespace SGJ.Player
{
    public class PlayerCombat : MonoBehaviour, IHittable, IPickerUp
    {
        [SerializeField] private float playerMaxHealth;
        private float _playerCurrentHealth;

        public float PlayerCurrentHealth
        {
            get => _playerCurrentHealth;
            set
            {
                _playerCurrentHealth = value;
                if (_playerCurrentHealth < 0f)
                {
                    // handle player death
                    _playerCurrentHealth = 0f;
                }

                OnPlayerHealthUpdated(this, _playerCurrentHealth);
            }
        }

        private int _healthKitsLeft;

        public EventHandler<float> OnPlayerHealthUpdated = delegate { };

        private void Start()
        {
            PlayerCurrentHealth = playerMaxHealth; 
        }

        public void UseMedKit()
        {
            if (_healthKitsLeft > 0)
                OnEntityGotHit(float.NegativeInfinity);
        }

        public void OnEntityGotHit(float incomeDamage) => _playerCurrentHealth -= incomeDamage;

        public void AddItemsInInventory(DroppedItem item)
        {
            // Check if item is medkit and if so -> add to medkits
        }
    }
}