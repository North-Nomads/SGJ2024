using SGJ.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace SGJ.Player
{
    public class PlayerUICanvas : MonoBehaviour
    {
        [SerializeField] private Image healthBarFiller;
        [SerializeField] private Text ammoCapacityText;

        public void UpdateFiller(object sender, float value)
        {
            value = Mathf.Clamp01(value);   
            healthBarFiller.fillAmount = value;
        }

        public void UpdateAmmoText(int ammoLeft) => ammoCapacityText.text = ammoLeft.ToString();
    }
}