using UnityEngine;
using UnityEngine.Pool;

namespace SGJ.Weapons.Instances
{
    public class Minigun : MonoBehaviour, IGeneralWeapon
    {
        /*[Header("GunStats")]
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float initialBulletSpread;
        [SerializeField] private float finalBulletSpread;
        [SerializeField] private float initalFireDelay;
        [SerializeField] private float finalFireDelay;
        [SerializeField] private float timeToRampUp;
        [SerializeField] private float timeToIncreaseAccuracy;
        [SerializeField] private float ammo;
        [SerializeField] private float flashTickDuration;*/
        private ObjectPool<IProjectile> _pool;
        private Animator _animator;

        public WeaponModel Weapon { set => throw new System.NotImplementedException(); }
        public int AmmoLeft { set => throw new System.NotImplementedException(); }
        public IProjectile WeaponProjectile { set => throw new System.NotImplementedException(); }
        public ObjectPool<IProjectile> ProjectilePool => _pool;
        public Animator GunAnimator => _animator;

        public void TryPerformAttack()
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            //_pool = new(SpawnBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, true, 30, 70);

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

        }

        private Bullet SpawnBullet()
        {
            float bulletSpread = Mathf.Lerp(initialBulletSpread, finalBulletSpread, _timeShooting / timeToIncreaseAccuracy);
            Vector3 speed = _aimDirection * bulletSpeed + Vector3.Cross(_aimDirection, Vector3.up).normalized * Random.Range(-bulletSpread, bulletSpread);
            Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(speed, Vector3.up));
            bullet.Pool = _pool;
            bullet.Speed = speed;
            return bullet;
        }

        private void OnGetBullet(Bullet bullet)
        {
            float bulletSpread = Mathf.Lerp(initialBulletSpread, finalBulletSpread, _timeShooting / timeToIncreaseAccuracy);
            Vector3 speed = _aimDirection * bulletSpeed + Vector3.Cross(_aimDirection, Vector3.up).normalized * Random.Range(-bulletSpread, bulletSpread);
            bullet.transform.SetPositionAndRotation(gunPoint.position, Quaternion.LookRotation(speed, Vector3.up));
            bullet.Pool = _pool;
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
        }*/
    }
}