using SGJ.Weapons;
using System;
using UnityEngine;

namespace SGJ.Player
{
    [RequireComponent(typeof(PlayerCamera))]
    public class PlayerWeaponry : MonoBehaviour
    {
        private PlayerCamera _playerCamera;
        private IGeneralWeapon _mainWeapon;
        private IMeleeWeapon _meleeWeapon;
        private Transform _weaponRootPosition;

        public EventHandler<IWeapon> OnPlayerShoot = delegate { };

        private void Start()
        {
            _playerCamera = GetComponent<PlayerCamera>();
        }

        public void ShootEquippedWeapon()
        {
            _mainWeapon.TryPerformAttack();
        }

        public void PerformMeleeAttack()
        {
            _meleeWeapon.TryPerformAttack();
        }

        public void EquipWeapon(IGeneralWeapon weapon)
        {
            _mainWeapon = weapon;
        }
    }
}