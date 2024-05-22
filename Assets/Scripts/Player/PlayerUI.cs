using System;
using UnityEngine;

namespace SGJ.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private PlayerUICanvas playerUIPrefab;
        public EventHandler<float> OnPlayerHealthChanged = delegate { };

        public void HandlePlayerHealthUpdated(float newHeakth)
        {

        }

        public void HandlePlayerAmmoUpdated(float newAmmo)
        {

        }

        /*public void Initialize()
        {
            _canvasInstance = Instantiate(playerUIPrefab, transform);
            OnPlayerHealthChanged += _canvasInstance.UpdateFiller;
        }

        public void UpdateInventoryHUD(int currentAmmo, int medkits)
        {
            if (currentAmmo == _lastAmmoCapacity && _lastMedKits == medkits)
                return;

            _lastAmmoCapacity = currentAmmo;
            _lastMedKits = medkits;
            _canvasInstance.UpdateAmmoText(currentAmmo);
            _canvasInstance.UpdateMedKitText(medkits);
        }*/
    }
}