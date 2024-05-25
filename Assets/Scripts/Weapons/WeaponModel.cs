using SGJ.Projectiles;
using System;
using UnityEngine;

namespace SGJ.Weapons
{
    [CreateAssetMenu(fileName = "Weapon")]
    public class WeaponModel : ScriptableObject
    {
        [SerializeField, Min(0)] private float hitDamage;
        [SerializeField, Range(0, 180)] private float spreadAngle;
        [SerializeField, Min(0)] private float projectileSpeed;
        [SerializeField, Min(0)] private int initialAmmo;
        [SerializeField, Min(0)] private float nextAttackDelay;
        [SerializeField] private float forwardImpulseStrength;
        [SerializeField] private float mobKnockbackStrength;
        [SerializeField] private Projectile weaponProjectile;

        public float HitDamage => hitDamage;
        public float SpreadAngle => spreadAngle;
        public float ProjectileSpeed => projectileSpeed;
        public int InitialAmmo => initialAmmo;
        public float NextAttackDelay => nextAttackDelay;
        public float ForwardImpulseStrength => forwardImpulseStrength;
        public float MobKnockbackStrength => mobKnockbackStrength;
        public IProjectile WeaponProjectile { get => weaponProjectile; }
    }
}