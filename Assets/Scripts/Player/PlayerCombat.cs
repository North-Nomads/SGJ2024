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

        public float MaxHealth => throw new NotImplementedException();

        public float CurrentHealth => throw new NotImplementedException();

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

        public void HandleHit()
        {
            throw new NotImplementedException();
        }

        public void DisplayHitImpact()
        {
            throw new NotImplementedException();
        }

        public void HandleHit(float incomeDamage)
        {
            throw new NotImplementedException();
        }

        public void HandleDeath()
        {
            throw new NotImplementedException();
        }
    }
}