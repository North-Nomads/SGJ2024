using SGJ.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace SGJ.Player
{
    public class PlayerUICanvas : MonoBehaviour
    {
        [SerializeField] private Image healthBarFiller;
        [SerializeField] private Text ammoCapacityText;
        [SerializeField] private RectTransform hatchPanel;
        [SerializeField] private Text hatchDescriptionText;

        public void UpdateFiller(object sender, float value)
        {
            value = Mathf.Clamp01(value);   
            healthBarFiller.fillAmount = value;
        }

        public void UpdateAmmoText(int ammoLeft) => ammoCapacityText.text = ammoLeft.ToString();

        public void EnableHatchPanel(NextLevelHatch hatch)
        {
            hatchPanel.gameObject.SetActive(true);
            hatchDescriptionText.text = hatch.GetHatchInfo;
        }

        public void DisableHatchPanel()
        {
            hatchPanel.gameObject.SetActive(false);
        }
    }
}