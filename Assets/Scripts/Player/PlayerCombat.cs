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
        private int _healthKitsLeft;

        public EventHandler<float> OnPlayerHealthUpdated = delegate { };
        public EventHandler<PlayerCombat> OnPlayerDied = delegate { };

        public bool IsPlayerAlive => _playerCurrentHealth < 0f;
        public float CurrentHealth
        {
            get => _playerCurrentHealth;
            set
            {
                /*_playerCurrentHealth = Mathf.Clamp(PlayerSaveController.DefaultPlayerHealth, 0, value);
                OnPlayerHealthUpdated(this, _playerCurrentHealth / PlayerSaveController.DefaultPlayerHealth);*/
                if (!IsPlayerAlive)
                    OnPlayerDied(this, this);
            }
        }
        public float MaxHealth => playerMaxHealth;

        private void Start()
        {
            CurrentHealth = MaxHealth; 
        }

        public void UseMedKit()
        {
            if (_healthKitsLeft > 0)
            {
                OnEntityGotHit(float.NegativeInfinity);
                _healthKitsLeft--;
            }
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