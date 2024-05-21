using UnityEngine;
using UnityEngine.Pool;

namespace SGJ.Weapons
{
    public interface IGeneralWeapon : IWeapon
    {
        public ObjectPool<IProjectile> ProjectilePool { get; }
        public Animator GunAnimator { get; }
    }
}