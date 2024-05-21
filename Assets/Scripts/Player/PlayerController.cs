using SGJ.Combat;
using SGJ.GameItems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SGJ.Player
{
    //yeah, that's a Controller all-in-one script. Cry about it
    [RequireComponent(typeof(PlayerUI))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform gunPoint;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Animator gunAnimator;

        private PlayerInventory _playerInventory;
        private float _currentPlayerHealth;
        private PlayerUI _playerUI;

        public int AmmoLeft => _playerInventory.Ammo;
        public int MedkitsLeft => _playerInventory.Medkits;
        public Dictionary<Items, int> PlayerInventory => _playerInventory.Inventory;

        private bool IsPlayerAlive => CurrentPlayerHealth > 0;
        public EventHandler<PlayerController> OnPlayerDied = delegate { };

        public float CurrentPlayerHealth
        {
            get => _currentPlayerHealth;
            private set
            {
                _currentPlayerHealth = Mathf.Clamp(PlayerSaveController.DefaultPlayerHealth, 0, value);
                _playerUI.OnPlayerHealthChanged(this, _currentPlayerHealth / PlayerSaveController.DefaultPlayerHealth);
                if (!IsPlayerAlive)
                    OnPlayerDied(this, this);
            }
        }

        private void Start()
        { 
            _playerUI = GetComponent<PlayerUI>();
            _playerUI.Initialize();

            _playerInventory = new PlayerInventory();
            CurrentPlayerHealth = PlayerSaveController.DefaultPlayerHealth;
        }

        private void Update()
        {
            if (!IsPlayerAlive)
                return;

            _playerUI.UpdateInventoryHUD(AmmoLeft, MedkitsLeft);
        }

    }
}