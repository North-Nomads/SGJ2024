using UnityEngine;

namespace SGJ.Combat
{
    public class AcceleratedWeaponTimer : IAcceleratedWeaponTimer
    {
        private readonly float _initalFireDelay;
        private readonly float _finalFireDelay;
        private readonly float _rampUpTime;

        private float _currentFireDelay;
        //currently locked at _rampUpTime;
        private float _currentShootingTime;
        private float _lastShotTime;

        public AcceleratedWeaponTimer(float initialFireDelay, float finalFireDelay, float rampUpTime)
        {
            _initalFireDelay = initialFireDelay;
            _finalFireDelay = finalFireDelay;
            _rampUpTime = rampUpTime;

            _currentFireDelay = initialFireDelay;
            _currentShootingTime = 0;
            _lastShotTime = float.MaxValue;
        }

        public float AccelerationProcentage => _currentShootingTime / _rampUpTime;

        public void OnGameTick()
        {
            if(_lastShotTime + _currentFireDelay > Time.time)
                _currentShootingTime -= Time.deltaTime;
        }

        public void OnShot()
        {
            _lastShotTime = Time.time;
            if (_currentShootingTime >= _rampUpTime)
                return;

            _currentShootingTime += _currentFireDelay;
            _currentFireDelay = Mathf.Lerp(_initalFireDelay, _finalFireDelay, AccelerationProcentage);
        }
    }
}