using SGJ.Weapons;
using System;
using UnityEngine;

namespace SGJ.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IProjectile
    {
        public abstract float Speed { get; set; }
        public abstract Vector3 FlyDirection { get; set; }

        public abstract event EventHandler OnHitEvent;

        public abstract void KeepMoving();

        public abstract IProjectile OnInstantiated(Vector3 position, float speed, Vector3 normalizedFlyDirection);

        public abstract void OnReactivated(Vector3 position, float speed, Vector3 normalizedFlyDirection);
    }
}
