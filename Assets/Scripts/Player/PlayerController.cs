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
    public class PlayerController : MonoBehaviour, IHittable
    {
        [Header("Player Stats")]
        [SerializeField] private float playerSpeed;

        [Header("References")]
        [SerializeField] Transform gunPoint;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Animator gunAnimator;

        private PlayerInventory _playerInventory;
        private float _currentPlayerHealth;
        private ObjectPool<Bullet> _bulletPool;
        private Vector3 _aimDirection;
        private float _shotCoolDown;
        private float _timeShooting;
        private Camera _playerCamera;
        private PlayerUI _playerUI;
        private CameraManager _cameraManager;
        private Light _muzzleFlash;
        private Light _spotLight;
        private float _flashTickGlowTimeLeft;

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
            _playerCamera = Camera.main;

            _playerUI = GetComponent<PlayerUI>();
            _playerUI.Initialize();

            _playerInventory = new PlayerInventory();
            CurrentPlayerHealth = PlayerSaveController.DefaultPlayerHealth;

            _cameraManager = _playerCamera.gameObject.GetComponent<CameraManager>();
            _muzzleFlash = gunPoint.GetChild(0).GetComponent<Light>();
            _spotLight = gunPoint.GetChild(1).GetComponent<Light>();
        }

        private void Update()
        {
            if (!IsPlayerAlive)
                return;

            _playerUI.UpdateInventoryHUD(AmmoLeft, MedkitsLeft);
            HandleMedkitUsage();
        }

        private void HandleMedkitUsage()
        {
            if (Input.GetKeyDown(KeyCode.F))
                if (_playerInventory.Inventory[Items.Medkit] > 0)
                    UseMedKit();

            void UseMedKit()
            {
                OnEntityGotHit(float.NegativeInfinity);
                _playerInventory.Inventory[Items.Medkit]--;
            }
        }

        public void OnEntityGotHit(float incomeDamage) => CurrentPlayerHealth -= incomeDamage;
    }
}