using System;
using UnityEngine;

namespace SGJ.Weapons
{
    public interface IProjectile
    {
        public float Speed { get; set; }
        public Vector3 FlyDirection { get; set; }
        public void OnInstantiated(Vector3 position, float speed, Vector3 normalizedFlyDirection);
        public void KeepMoving();

        public event EventHandler OnHitEvent; 
    }
}