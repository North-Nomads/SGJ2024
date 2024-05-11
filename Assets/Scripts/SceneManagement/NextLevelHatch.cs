using SGJ.GameItems;
using SGJ.Player;
using System;
using System.Collections.Generic;
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

        [SerializeField] private LevelDifficulty difficulty;
        [SerializeField] private bool isHubHatch;

        private Animator _animator;
        private string _analysisInfo;
        private string _sonarInfo;
        private bool _isPlayerNear;

        public string GetHatchInfo => $"Угроза: {_analysisInfo}\nАнализ форм жизней: {_sonarInfo}";
        public EventHandler<LevelDifficulty> OnHatchTriggered = delegate { };
        
        private Dictionary<LevelDifficulty, string> _sonarPhrases;
        private Dictionary<LevelDifficulty, string> _lifeFormPhrases;

        private void Start()
        {
            if (!isHubHatch)
                _animator = GetComponentInChildren<Animator>();

            _sonarPhrases = new Dictionary<LevelDifficulty, string>
            {
                { LevelDifficulty.Peace, "Не обнаружена" },
                { LevelDifficulty.Easy, "Обнаружена" },
                { LevelDifficulty.Medium, "Умеренная" },
                { LevelDifficulty.Hard, "Высокая активность" }
            };
            _lifeFormPhrases = new Dictionary<LevelDifficulty, string>
            {
                { LevelDifficulty.Peace, "Не проведен" },
                { LevelDifficulty.Easy, "<50" },
                { LevelDifficulty.Medium, ">100" },
                { LevelDifficulty.Hard, ">300" }
            };
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
