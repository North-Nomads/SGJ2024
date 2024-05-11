using SGJ.SceneManagement;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SGJ.Player
{
    public class PlayerUICanvas : MonoBehaviour
    {
        [SerializeField] private Image healthBarFiller;
        [SerializeField] private Text ammoCapacityText;
        [SerializeField] private Text medkitsCapacityText;

        public void UpdateFiller(object sender, float value)
        {
            value = Mathf.Clamp01(value);   
            healthBarFiller.fillAmount = value;
        }

        public void UpdateAmmoText(int ammoLeft) => ammoCapacityText.text = ammoLeft.ToString();

        public void UpdateMedKitText(int medkits) => medkitsCapacityText.text = medkits.ToString();
    }
}