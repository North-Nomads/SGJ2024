using System;
using UnityEngine;
using UnityEngine.UI;

namespace SGJ.Player
{
    public class PlayerUICanvas : MonoBehaviour
    {
        [SerializeField] private Image healthBarFiller;

        public void UpdateFiller(object sender, float value)
        {
            value = Mathf.Clamp01(value);   
            healthBarFiller.fillAmount = value;
        }
    }
}