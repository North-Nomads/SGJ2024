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
            {
                if (!isHubHatch)
                    other.GetComponent<PlayerUI>().ToggleHatchPanel(true, this);
                _isPlayerNear = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!isHubHatch)
                    other.GetComponent<PlayerUI>().ToggleHatchPanel(false, this);
                _isPlayerNear = false;
            }
        }

        private void Update()
        {
            if (!isHubHatch)
                _animator.SetBool("IsOpened", _isPlayerNear);

            if (_isPlayerNear)
            {
                CheckInventoryUsage();

                if (Input.GetKeyDown(KeyCode.E))
                    OnHatchWasChosen();

            }
        }

        private void CheckInventoryUsage()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                HandleInventoryItem(Items.LifeAnalizer);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                HandleInventoryItem(Items.Sonar);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                HandleInventoryItem(Items.Grenade);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                HandleInventoryItem(Items.Dynamite);
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

        private void HandleInventoryItem(Items item)
        {
            switch (item)
            {
                case Items.Sonar:
                    _sonarInfo = _sonarPhrases[difficulty];
                    return;
                case Items.LifeAnalizer:
                    _analysisInfo = _lifeFormPhrases[difficulty];
                    return;
                case Items.Grenade:
                    difficulty = GetLowerDifficulty(1);
                    return;
                case Items.Dynamite:
                    difficulty = GetLowerDifficulty(2);
                    return;
                default:
                    return;
            }

            LevelDifficulty GetLowerDifficulty(int step)
            {
                var currentDifficulty = (int)difficulty - step;
                if (currentDifficulty <= 0)
                    return LevelDifficulty.Peace;
                return (LevelDifficulty)currentDifficulty;

            }
        }
    }
}
