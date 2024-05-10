using SGJ.Mobs;
using SGJ.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGJ.SceneManagement
{
    public class GoalObserver : MonoBehaviour
    {
        private const string MobSpawnPointTag = "MobSpawnPoint";
        private const string PathToHatch = "Prefabs/Props/Hatches/";
        private const string HatchSpawnPointTag = "HatchSpawnPoint";
        [SerializeField] private float delayBeforeReturnToHub = 3f;
        [SerializeField] private float delayBetweenWaves;
        [SerializeField] private bool isHubLocation;

        private readonly List<NextLevelHatch> _hatchInstances = new();

        private AssetSpawner _assetSpawner;
        private MobSpawner _mobSpawner;
        private GameObject[] _spawnPoints;
        private PlayerController _player;
        private int _currentWaveIndex;
        private int _wavesThisMission;
        private bool _isLevelCleared;

        private void Start()
        {
            print(PlayerSaveController.UpcomingDifficulty);
            _assetSpawner = new AssetSpawner(this);
            _currentWaveIndex = 0;
            _wavesThisMission = (int)PlayerSaveController.UpcomingDifficulty;
            _player = _assetSpawner.Player;

            var isMissionPeacful = PlayerSaveController.UpcomingDifficulty == LevelDifficulty.Peace;

            _isLevelCleared = isMissionPeacful;

            if (isHubLocation)
            {
                var hubHatch = GameObject.FindGameObjectWithTag("HubHatch").GetComponent<NextLevelHatch>();
                hubHatch.OnHatchTriggered += HandleHatchWasChosen;
                return;
            }

            SpawnHatches();

            if (isMissionPeacful)
                return;

            InstantiateMobs();
            LaunchWavesLoop();

            void SpawnHatches()
            {
                var spawnPosition = GameObject.FindGameObjectWithTag(HatchSpawnPointTag).transform;
                var allHatchesCombos = Resources.LoadAll(PathToHatch);
                var hatchesGroup = Instantiate(allHatchesCombos[Random.Range(0, allHatchesCombos.Length)], spawnPosition);
                var hatches = hatchesGroup.GetComponentsInChildren<NextLevelHatch>();

                foreach (var hatch in hatches)
                {
                    hatch.OnHatchTriggered += HandleHatchWasChosen;
                    _hatchInstances.Add(hatch);
                }
            }
        }

        private void HandleHatchWasChosen(object sender, LevelDifficulty e)
        {
            if (!_isLevelCleared)
                return;

            print($"Sender: {sender}");
            PlayerSaveController.UpcomingDifficulty = e;

            if (isHubLocation)
                return;

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

        public void HandleWaveCleaned()
        {
            if (_currentWaveIndex == _wavesThisMission)
            {
                PlayerSaveController.SavePlayerProgress(_player.CurrentPlayerHealth, _player.PlayerInventory);
                _isLevelCleared = true;
                return;
            }

            print($"Launching wave {_currentWaveIndex++}/{_wavesThisMission}");
            LaunchWavesLoop();
        }
    }
}
