using System;
using UnityEngine;

namespace SGJ.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private PlayerUICanvas playerUIPrefab;
        private PlayerUICanvas _canvasInstance;
        private int _lastAmmoCapacity;

        public EventHandler<float> OnPlayerHealthChanged = delegate { };

        public void Initialize()
        {
            _canvasInstance = Instantiate(playerUIPrefab, transform);
            OnPlayerHealthChanged += _canvasInstance.UpdateFiller;
        }

        public void UpdateAmmoInHUD(int currentAmmo)
        {
            if (currentAmmo == _lastAmmoCapacity)
                return;

            _lastAmmoCapacity = currentAmmo;
            _canvasInstance.UpdateAmmoText(currentAmmo);
        }

    }
}