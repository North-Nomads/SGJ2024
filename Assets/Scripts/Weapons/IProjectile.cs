using System;
using UnityEngine;

namespace SGJ.Weapons
{
    public interface IProjectile
    {
        public float Speed { get; set; }
        public Vector3 FlyDirection { get; set; }
        public IProjectile OnInstantiated(Vector3 position, float speed, Vector3 normalizedFlyDirection);
        public void OnReactivated(Vector3 position, float speed, Vector3 normalizedFlyDirection);
        public void KeepMoving();

        public event EventHandler OnHitEvent; 
    }
}