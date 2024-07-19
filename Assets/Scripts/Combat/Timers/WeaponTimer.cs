using UnityEngine;

namespace SGJ.Combat
{
    public class WeaponTimer : IWeaponTimer
    {
        private float lastShotTime;

        public bool ReadyToFire => throw new System.NotImplementedException();

        public void OnGameTick()
        {
            throw new System.NotImplementedException();
        }

        public void OnShot()
        {
            throw new System.NotImplementedException();
        }
    }
}