using SGJ.Combat;
using SGJ.Weapons.Modifiers;
using UnityEngine;
using UnityEngine.Pool;

namespace SGJ.Weapons.Instances
{
    [RequireComponent(typeof(Animator))]
    public class Minigun : MonoBehaviour, IGeneralWeapon, IAcceleratedWeapon, IPrecisedWeapon
    {
        [Header("GunStats")]
        //[SerializeField] private float bulletSpeed;
        [SerializeField] private float initialBulletSpread;
        [SerializeField] private float finalBulletSpreadAngle;
        [SerializeField] private float initalFireDelay;
        [SerializeField] private float finalFireDelay;
        [SerializeField] private float timeToRampUp;
        [SerializeField] private float timeToIncreaseAccuracy;
        //[SerializeField] private float ammo;
        //[SerializeField] private float flashTickDuration;
        [SerializeField] private GameObject gunPoint;

        [SerializeField] WeaponModel model;
 
        private ObjectPool<IProjectile> _pool;
        private Animator _animator;
        private IAcceleratedWeaponTimer _fireRateTimer;
        private IAcceleratedWeaponTimer _accuracyTimer;

        public WeaponModel Weapon { get => model; }
        public int AmmoLeft { set => throw new System.NotImplementedException(); }
        public IProjectile WeaponProjectile { set => throw new System.NotImplementedException(); }
        public ObjectPool<IProjectile> ProjectilePool => _pool;
        public Animator GunAnimator => _animator;

        public float FinalFireDelay => finalFireDelay;

        public float TimeToRampUp => timeToRampUp;

        public float FinalSpreadAngle => finalBulletSpreadAngle;

        public float TimeToIncreaseAccuracy => timeToIncreaseAccuracy;

        public void TryPerformAttack()
        {
            if(_fireRateTimer.ReadyToFire)
            {
                _pool.Get();
                _fireRateTimer.OnShot();
            }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _fireRateTimer = new AcceleratedWeaponTimer(initalFireDelay, finalFireDelay, timeToRampUp);
            _pool = new(SpawnBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, true, 0, 70);

        }
        private void Update()
        {
            _fireRateTimer.OnGameTick();
        }

        /*private void Shoot()
        {
            gunAnimator.SetBool("IsFiring", Input.GetMouseButton(0));

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


            if (!Input.GetMouseButton(0))
            {
                _timeShooting = _timeShooting > 0 ? _timeShooting - 2 * Time.deltaTime : 0;
                return;
            }

            _pool.Get();
            _playerInventory.Inventory[Items.Ammo]--;
            if (_timeShooting < timeToIncreaseAccuracy + timeToRampUp)
                _timeShooting += Mathf.Lerp(initalFireDelay, finalFireDelay,
                    Mathf.Clamp01(_timeShooting / timeToRampUp));
            _shotCoolDown = Mathf.Lerp(initalFireDelay, finalFireDelay,
                Mathf.Clamp01(_timeShooting / timeToRampUp));
            _cameraManager.ShakeCamera(Random.onUnitSphere * 0.08f);
            _flashTickGlowTimeLeft = flashTickDuration;

        }*/

        private IProjectile SpawnBullet()
        {
            float bulletSpread = Mathf.Lerp(Mathf.Tan(Weapon.SpreadAngle) / 2,
                Mathf.Tan(FinalSpreadAngle) / 2, _fireRateTimer.AccelerationProcentage);
            Vector3 direction = transform.forward + Vector3.Cross(transform.forward, Vector3.up).normalized * Random.Range(-bulletSpread, bulletSpread);
            IProjectile bulletPrefab = Weapon.WeaponProjectile;
            var bulletInstance = bulletPrefab.OnInstantiated(gunPoint.transform.position, Weapon.ProjectileSpeed, direction.normalized);
            bulletInstance.OnHitEvent += OnBulletHit;
            return bulletInstance;
        }


        private void OnGetBullet(IProjectile bullet)
        {
            float bulletSpread = Mathf.Lerp(Mathf.Tan(Weapon.SpreadAngle) / 2,
                Mathf.Tan(FinalSpreadAngle) / 2, _fireRateTimer.AccelerationProcentage);
            Vector3 direction = transform.forward + Vector3.Cross(transform.forward, Vector3.up).normalized * Random.Range(-bulletSpread, bulletSpread);
            bullet.OnReactivated(transform.position, Weapon.ProjectileSpeed, direction.normalized);
            (bullet as Bullet).transform.SetPositionAndRotation(gunPoint.transform.position, Quaternion.LookRotation(direction));
            (bullet as Bullet).gameObject.SetActive(true);
        }

        private void OnReleaseBullet(IProjectile bullet)
        {
            (bullet as Bullet).gameObject.SetActive(false);
        }

        private void OnDestroyBullet(IProjectile bullet)
        {
            Destroy((bullet as Bullet).gameObject);
        }

        private void OnBulletHit(object sender, System.EventArgs e)
        {
            _pool.Release(sender as IProjectile);
        }
    }
}