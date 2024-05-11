using SGJ.Player;
using System;
using UnityEngine;

namespace SGJ.SceneManagement
{
    public enum LevelDifficulty
    {
        Peace = 0,
        Easy = 2,
        Medium = 4,
        Hard = 6
    }

    public class NextLevelHatch : MonoBehaviour
    {
        private const int CombatSceneID = 1;

        [SerializeField] private LevelDifficulty difficulty;
        [SerializeField] private bool isHubHatch;

        private Animator _animator;
        private bool _isPlayerNear;

        public EventHandler<LevelDifficulty> OnHatchTriggered = delegate { };

        private void Start()
        {
            if (!isHubHatch)
                _animator = GetComponentInChildren<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                _isPlayerNear = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                _isPlayerNear = false;
        }

        private void Update()
        {
            if (!isHubHatch)
                _animator.SetBool("IsOpened", _isPlayerNear);

            if (_isPlayerNear)
            {
                if (Input.GetKeyDown(KeyCode.E))
                    OnHatchWasChosen();
            }
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
