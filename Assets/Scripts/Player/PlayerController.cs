using SGJ.Combat;
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
        [SerializeField] private float playerHealth;

        [Header("GunStats")]
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float bulletSpread;
        [SerializeField] private float initalFireDelay;
        [SerializeField] private float finalFireDelay;
        [SerializeField] private float timeToRampUp;
        [SerializeField] private float ammo;
        [SerializeField]private float flashTickDuration;
        [Header("References")]
        [SerializeField] Transform gunPoint;
        [SerializeField] private Bullet bulletPrefab;

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
        private float _flashTickGlowTimeLeft;

        private bool IsPlayerAlive => CurrentPlayerHealth > 0;
        public System.EventHandler<PlayerController> OnPlayerDied = delegate { };

        public float CurrentPlayerHealth
        {
            get => _currentPlayerHealth;
            private set
            {
                _currentPlayerHealth = value;
                _playerUI.OnPlayerHealthChanged(this, _currentPlayerHealth / playerHealth);
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
            CurrentPlayerHealth = playerHealth;
            _cameraManager = _playerCamera.gameObject.GetComponent<CameraManager>();
            _muzzleFlash = gunPoint.GetChild(0).GetComponent<Light>();
        }

        private void Update()
        {
            if (!IsPlayerAlive)
                return;

            Look();
            Move();
            Shoot();
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
            _shotCoolDown -= Time.deltaTime;
            _flashTickGlowTimeLeft -= Time.deltaTime;
            if (_flashTickGlowTimeLeft > 0)
                _muzzleFlash.enabled = true;
            else
                _muzzleFlash.enabled = false;

            if (_shotCoolDown > 0 || ammo <= 0)
                return;
            if(!Input.GetMouseButton(0))
            {
                _timeShooting = 0;
                return;
            }
            
            _bulletPool.Get();
            ammo--;
            _timeShooting += Mathf.Lerp(initalFireDelay, finalFireDelay,
                Mathf.Clamp01(_timeShooting / timeToRampUp));
            _shotCoolDown = Mathf.Lerp(initalFireDelay, finalFireDelay,
                Mathf.Clamp01(_timeShooting / timeToRampUp));
            _cameraManager.ShakeCamera(Random.onUnitSphere * 0.1f);
        }

        private Bullet SpawnBullet()
        {
            Vector3 speed = _aimDirection * bulletSpeed + Vector3.Cross(_aimDirection, Vector3.up).normalized * Random.Range(-bulletSpread, bulletSpread);
            Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(speed, Vector3.up));
            bullet.Pool = _bulletPool;
            _flashTickGlowTimeLeft = flashTickDuration;
            bullet.Speed = speed;
            return bullet;
        }

        private void OnGetBullet(Bullet bullet)
        {
            Vector3 speed = _aimDirection * bulletSpeed + Vector3.Cross(_aimDirection, Vector3.up).normalized * Random.Range(-bulletSpread, bulletSpread);
            bullet.transform.SetPositionAndRotation(gunPoint.position, Quaternion.LookRotation(speed, Vector3.up));
            bullet.Pool = _bulletPool;
            bullet.Speed = speed;
            _flashTickGlowTimeLeft = flashTickDuration;
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

        public void OnEntityGotHit(float incomeDamage)
        {
            CurrentPlayerHealth -= incomeDamage;
            print($"Player health: {playerHealth}");
        }
    }

}