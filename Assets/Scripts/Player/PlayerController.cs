using SGJ.Combat;
using SGJ.GameItems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace SGJ.Player
{
    //yeah, that's a Controller all-in-one script. Cry about it
    [RequireComponent(typeof(PlayerUI))]
    public class PlayerController : MonoBehaviour, IHittable
    {
        [Header("Player Stats")]
        [SerializeField] private float playerSpeed;

        [Header("GunStats")]
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float initialBulletSpread;
        [SerializeField] private float finalBulletSpread;
        [SerializeField] private float initalFireDelay;
        [SerializeField] private float finalFireDelay;
        [SerializeField] private float timeToRampUp;
        [SerializeField] private float timeToIncreaseAccuracy;
        [SerializeField] private float ammo;
        [SerializeField]private float flashTickDuration;
        [Header("References")]
        [SerializeField] Transform gunPoint;
        [SerializeField] private Bullet bulletPrefab;

        private PlayerInventory _playerInventory;
        private float _currentPlayerHealth;
        private CharacterController _characterController;
        private ObjectPool<Bullet> _bulletPool;
        private Vector3 _cursorPostion;
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
            _characterController = GetComponent<CharacterController>();
            _bulletPool = new(SpawnBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, true, 30, 70);
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
            Look();
            Move();
            Shoot();
            HandleMedkitUsage();
        }

        private void HandleMedkitUsage()
        {
            if (Input.GetKey(KeyCode.F))
                if (_playerInventory.Inventory[Items.Medkit] > 0)
                    UseMedKit();

            void UseMedKit()
            {
                OnEntityGotHit(float.PositiveInfinity);
                _playerInventory.Inventory[Items.Medkit]--;
            }
        }

        private void Look()
        {
            Physics.Raycast(_playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
            _cursorPostion = hit.point;
            transform.LookAt(new Vector3(_cursorPostion.x, transform.position.y, _cursorPostion.z));
            _aimDirection = (new Vector3(_cursorPostion.x, transform.position.y, _cursorPostion.z) - transform.position).normalized;
        }

        private void Move()
        {
            Vector3 movementVector = Vector3.ClampMagnitude(new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1);
            _characterController.SimpleMove(movementVector * playerSpeed);
        }
        private void Shoot()
        {
            _spotLight.spotAngle = (180 / Mathf.PI) * 4 * 
                Mathf.Atan(Mathf.Lerp(initialBulletSpread, finalBulletSpread, _timeShooting / timeToIncreaseAccuracy) / bulletSpeed);
            _spotLight.innerSpotAngle = _spotLight.spotAngle / 2;
            _shotCoolDown -= Time.deltaTime;
            _flashTickGlowTimeLeft -= Time.deltaTime;
            if (_flashTickGlowTimeLeft > 0)
                _muzzleFlash.enabled = true;
            else
                _muzzleFlash.enabled = false;

            if (_shotCoolDown > 0 || AmmoLeft <= 0)
                return;
            if(!Input.GetMouseButton(0))
            {
                _timeShooting = _timeShooting > 0 ? _timeShooting - 2 * Time.deltaTime : 0;
                return;
            }
            
            _bulletPool.Get();
            _playerInventory.Inventory[Items.Ammo]--;
            _timeShooting += Mathf.Lerp(initalFireDelay, finalFireDelay,
                Mathf.Clamp01(_timeShooting / timeToRampUp));
            _shotCoolDown = Mathf.Lerp(initalFireDelay, finalFireDelay,
                Mathf.Clamp01(_timeShooting / timeToRampUp));
            _cameraManager.ShakeCamera(Random.onUnitSphere * 0.1f);
            _flashTickGlowTimeLeft = flashTickDuration;
            
        }

        private Bullet SpawnBullet()
        {
            float bulletSpread = Mathf.Lerp(initialBulletSpread, finalBulletSpread, _timeShooting / timeToIncreaseAccuracy);
            Vector3 speed = _aimDirection * bulletSpeed + Vector3.Cross(_aimDirection, Vector3.up).normalized * Random.Range(-bulletSpread, bulletSpread);
            Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(speed, Vector3.up));
            bullet.Pool = _bulletPool;
            bullet.Speed = speed;
            return bullet;
        }

        private void OnGetBullet(Bullet bullet)
        {
            float bulletSpread = Mathf.Lerp(initialBulletSpread, finalBulletSpread, _timeShooting / timeToIncreaseAccuracy);
            Vector3 speed = _aimDirection * bulletSpeed + Vector3.Cross(_aimDirection, Vector3.up).normalized * Random.Range(-bulletSpread, bulletSpread);
            bullet.transform.SetPositionAndRotation(gunPoint.position, Quaternion.LookRotation(speed, Vector3.up));
            bullet.Pool = _bulletPool;
            bullet.Speed = speed;
            bullet.gameObject.SetActive(true);
        }

        private void OnReleaseBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void OnDestroyBullet(Bullet bullet)
        {
            Destroy(bullet.gameObject);
        }

        public void OnEntityGotHit(float incomeDamage) => CurrentPlayerHealth -= incomeDamage;
    }
}