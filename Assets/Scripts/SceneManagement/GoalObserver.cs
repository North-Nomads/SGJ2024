using Cinemachine;
using SGJ.Mobs;
using SGJ.Player;
using System;
using System.Collections;
using UnityEngine;

namespace SGJ.SceneManagement
{
    public class GoalObserver : MonoBehaviour
    {
        private const string MobSpawnPointTag = "MobSpawnPoint";

        [SerializeField] private int wavesQuantity;
        [SerializeField] private float delayBetweenWaves;
        [SerializeField] private bool isPeacefulLocation;
        [SerializeField] private float delayBeforeReturnToHub = 3f;        

        private AssetSpawner _assetSpawner;
        private MobSpawner _mobSpawner;
        private GameObject[] _spawnPoints;
        private PlayerController _player;
        private int _currentWaveIndex;
        private int _wavesThisMission;

        private void Start()
        {
            _assetSpawner = new AssetSpawner(this);
            _currentWaveIndex = 0;
            _wavesThisMission = (int)PlayerSaveController.UpcomingDifficulty;
            _player = _assetSpawner.Player;

            if (isPeacefulLocation)
                return;

            InstantiateMobs();
            LaunchWavesLoop();
        }

        public void HandlePlayerDied(object sender, PlayerController player)
        {
            PlayerSaveController.ResetPlayerProgress();
            StartCoroutine(ReturnToHubAfterDelay());

        }

        private IEnumerator ReturnToHubAfterDelay()
        {
            yield return new WaitForSeconds(delayBeforeReturnToHub);
            SceneController.LoadScene(0);
        }

        private void InstantiateMobs()
        {
            _spawnPoints = GameObject.FindGameObjectsWithTag(MobSpawnPointTag);
            _mobSpawner = new MobSpawner(this, _spawnPoints, _player);
        }

        private void LaunchWavesLoop()
        {
            _currentWaveIndex++;
            _mobSpawner.TriggerNewWaveAfterDelay(delayBetweenWaves);
        }

        private void HandleLevelGoalAchieved()
        {
            if (PlayerSaveController.IsCurrentMissionLastOne)
            {
                PlayerSaveController.ResetPlayerProgress();
                StartCoroutine(ReturnToHubAfterDelay());
                return;
            }

            HandlePlayerNextMissionChoose();
        }

        public void HandlePlayerNextMissionChoose()
        {
            PlayerSaveController.CurrentMissionIndex++;
            SceneController.LoadScene(1);
        }

        public void HandleWaveCleaned()
        {
            if (_currentWaveIndex == _wavesThisMission)
            {
                PlayerSaveController.SavePlayerProgress(_player.CurrentPlayerHealth, _player.PlayerInventory);
                HandleLevelGoalAchieved();
                return;
            }

            LaunchWavesLoop();
        }
    }
}
