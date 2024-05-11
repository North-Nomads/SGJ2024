using SGJ.Player;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        public const string HatchUICanvasPath = "Prefabs/Props/Hatches/Canvas";
        public const string HubHatchUICanvasPath = "Prefabs/Props/Hatches/HubCanvas";

        private LevelDifficulty _difficulty;
        [SerializeField] private bool isHubHatch;

        private Animator _animator;
        private bool _isPlayerNear;

        public EventHandler<LevelDifficulty> OnHatchTriggered = delegate { };
        private Canvas _ui;

        private void Start()
        {
            if (!isHubHatch)
                _animator = GetComponentInChildren<Animator>();

            Array values = Enum.GetValues(typeof(LevelDifficulty));
            var startIndex = 0;
            if (PlayerSaveController.IsCurrentMissionLastOne)
                startIndex = 1;

            _difficulty = (LevelDifficulty)values.GetValue(Random.Range(startIndex, values.Length));

            var uiPrefab = Resources.Load<Canvas>(HatchUICanvasPath);
            if (isHubHatch)
                uiPrefab = Resources.Load<Canvas>(HubHatchUICanvasPath);


            _ui = Instantiate(uiPrefab);
            _ui.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerNear = true;
                _ui.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerNear = false;
                _ui.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!isHubHatch && !gameObject.CompareTag("HubHatch"))
                _animator.SetBool("IsOpened", _isPlayerNear && PlayerSaveController.IsLocationFinished);

            if (_isPlayerNear)
            {
                if (Input.GetKeyDown(KeyCode.E))
                    OnHatchWasChosen();
            }
        }

        private void OnHatchWasChosen()
        {
            OnHatchTriggered(gameObject, _difficulty);
            if (!isHubHatch)
                return;

            PlayerSaveController.ResetPlayerProgress();
            PlayerSaveController.LaunchNewMission();
            SceneController.LoadScene(CombatSceneID);
        }
    }
}
