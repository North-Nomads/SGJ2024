using SGJ.Mobs;
using SGJ.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGJ.SceneManagement
{
    public class GoalObserver : MonoBehaviour
    {
        private const string MobSpawnPointTag = "MobSpawnPoint";
        private const string PathToHatch = "Prefabs/Props/Hatches/Hatch";
        private const string HatchSpawnPointTag = "HatchSpawnPoint";
        private const string PathToHubCapsule = "Prefabs/Props/End Capsule";
        [SerializeField] private float delayBeforeReturnToHub = 3f;
        [SerializeField] private float delayBetweenWaves;
        [SerializeField] private bool isHubLocation;

        private AssetSpawner _assetSpawner;
        private GameObject[] _spawnPoints;
        private PlayerController _player;
        private MobSpawner _mobSpawner;
        private int _currentWaveIndex;
        private int _wavesThisMission;
        private bool _isLevelCleared;

        private void Start()
        {
            _assetSpawner = new AssetSpawner(this);
            _currentWaveIndex = 0;
            _wavesThisMission = (int)PlayerSaveController.UpcomingDifficulty;
            _player = _assetSpawner.Player;
            PlayerSaveController.IsLocationFinished = _wavesThisMission == 0;

            var isMissionPeacful = PlayerSaveController.UpcomingDifficulty == LevelDifficulty.Peace;

            _isLevelCleared = isMissionPeacful;

            if (isHubLocation)
            {
                var hubHatch = GameObject.FindGameObjectWithTag("HubHatch").GetComponent<NextLevelHatch>();
                hubHatch.OnHatchTriggered += HandleHatchWasChosen;
                return;
            }

            if (!PlayerSaveController.IsCurrentMissionLastOne)
                SpawnHatches();

            if (isMissionPeacful)
                return;

            InstantiateMobs();
            LaunchWavesLoop();

        }
        private void SpawnHatches()
        {
            var spawnPosition = GameObject.FindGameObjectWithTag(HatchSpawnPointTag).transform;
            var hatchPrefab = Resources.Load<NextLevelHatch>(PathToHatch);
            if (PlayerSaveController.IsCurrentMissionLastOne)
                hatchPrefab = Resources.Load<NextLevelHatch>(PathToHubCapsule);


            var hatchInstance = Instantiate(hatchPrefab, spawnPosition);
            hatchInstance.OnHatchTriggered += HandleHatchWasChosen;
        }

        private void HandleHatchWasChosen(object sender, LevelDifficulty e)
        {
            if (!_isLevelCleared)
                return;

            PlayerSaveController.UpcomingDifficulty = e;

            if (isHubLocation)
                return;

            if (PlayerSaveController.IsCurrentMissionLastOne)
            {
                PlayerSaveController.UpcomingDifficulty = LevelDifficulty.Peace;
                StartCoroutine(ReturnToHubAfterDelay());
                return;
            }

            PlayerSaveController.CurrentMissionIndex++;
            SceneController.LoadScene(1);
        }

        public void HandlePlayerDied(object sender, PlayerController player)
        {
            PlayerSaveController.ResetPlayerProgress();
            StartCoroutine(ReturnToHubAfterDelay());
        }

        private IEnumerator ReturnToHubAfterDelay()
        {
            PlayerSaveController.IsLocationFinished = false;
            yield return new WaitForSeconds(delayBeforeReturnToHub);
            PlayerSaveController.ResetPlayerProgress();
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

        public void HandleWaveCleaned()
        {
            if (_currentWaveIndex == _wavesThisMission)
            {
                PlayerSaveController.SavePlayerProgress(_player.CurrentPlayerHealth, _player.PlayerInventory);
                _isLevelCleared = true;
                PlayerSaveController.IsLocationFinished = true;
                SpawnHatches();
                return;
            }

            LaunchWavesLoop();
        }
    }
}
