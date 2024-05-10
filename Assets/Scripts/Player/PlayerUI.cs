using System;
using UnityEngine;

namespace SGJ.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private PlayerUICanvas playerUIPrefab;
        private PlayerUICanvas _canvasInstance;

        public EventHandler<float> OnPlayerHealthChanged = delegate { };

        private void Start()
        {
            _canvasInstance = Instantiate(playerUIPrefab, transform);
            OnPlayerHealthChanged += _canvasInstance.UpdateFiller;
        }
    }
}