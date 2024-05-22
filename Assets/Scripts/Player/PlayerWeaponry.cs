using SGJ.GameItems;
using SGJ.Infrastructure;
using SGJ.Proprs;
using SGJ.Weapons;
using System;
using UnityEngine;

namespace SGJ.Player
{
    [RequireComponent(typeof(PlayerCamera))]
    public class PlayerWeaponry : MonoBehaviour, IPickerUp, IGameService
    {
        [SerializeField] private Transform weaponRootPosition;
        
        private PlayerCamera _playerCamera;
        private IGeneralWeapon _mainWeapon;
        private IMeleeWeapon _meleeWeapon;

        public Transform WeaponRootPosition => weaponRootPosition;
            
        public EventHandler<IWeapon> OnPlayerShoot = delegate { };

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

        public void AddItemsInInventory(DroppedItem item)
        {
            // Check if is ammo and add to current weapon ammunition
        }

        public void OnServiceInstantiated()
        {
            ServiceLocator.Current.Register(this);
            _playerCamera = GetComponent<PlayerCamera>();
            _playerCamera.SetFollowObject(transform);
        }
    }
}