using SGJ.Player;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SGJ.SceneManagement
{
    public enum LevelDifficulty
    {
        Peace = 0,
        Easy = 3,
        Medium = 6,
        Hard = 8
    }

    public class NextLevelHatch : MonoBehaviour
    {
        private const int CombatSceneID = 1;
        private bool _isPlayerNear;

        [SerializeField] private LevelDifficulty difficulty;
        [SerializeField] private bool isHubHatch;

        public EventHandler<LevelDifficulty> OnHatchTriggered = delegate { };

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                _isPlayerNear = true;
        }

        private void Update()
        {
            if (_isPlayerNear)
                if (Input.GetKeyDown(KeyCode.E))
                    OnHatchWasChosen();
        }

        private void OnHatchWasChosen()
        {
            OnHatchTriggered(gameObject, difficulty);
            if (!isHubHatch)
                return;

            PlayerSaveController.ResetPlayerProgress();
            PlayerSaveController.LaunchNewMission();
            SceneController.LoadScene(CombatSceneID);
        }
    }
}
