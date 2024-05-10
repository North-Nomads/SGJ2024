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
        [SerializeField] private int wavesQuantity;
        [SerializeField] private int[] mobsInWaves;
        [SerializeField] private float delayBetweenWaves;
        [SerializeField] private bool isPeacefulLocation;
        [SerializeField] private float delayBeforeReturnToHub = 3f;

        private const string MobSpawnPoint = "MobSpawnPoint";
        private const string PlayerSpawnPoint = "PlayerSpawnPoint";
        private const string PlayerPrefabPath = "Prefabs/Player/Player";
        private const string PlayerCameraPath = "Prefabs/Player/Virtual Camera";

        private MobSpawner _mobSpawner;
        private GameObject[] _spawnPoints;
        private PlayerController _player;
        private int _currentWaveIndex;

        private void OnValidate()
        {
            if (mobsInWaves == null)
                mobsInWaves = new int[] { };

            if (wavesQuantity != mobsInWaves.Length)
                Debug.LogWarning("Waves Quantity != Mobs In Waves Array Size. Don't forget to set up waves array");
        }

        private void Start()
        {
            _currentWaveIndex = -1;

            InstantiatePlayer();
            InstantiateCamera();
            
            if (isPeacefulLocation)
                return;

            InstantiateMobs();
            LaunchWavesLoop();
        }

        private void Update()
        {
            // DEBUG ONLY
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _mobSpawner.KillAllMobs();
            }
        }

        private void InstantiatePlayer()
        {
            var player = Resources.Load<PlayerController>(PlayerPrefabPath);
            var playerSpawnPoint = GameObject.FindGameObjectWithTag(PlayerSpawnPoint);
            if (playerSpawnPoint == null)
                throw new Exception($"No player spawn point found. Assign spawn point object corresponding Tag: {PlayerSpawnPoint}");
            _player = Instantiate(player, playerSpawnPoint.transform.position, Quaternion.identity);
            _player.OnPlayerDied += HandlePlayerLose;
        }

        private void HandlePlayerLose(object sender, PlayerController player)
        {
            StartCoroutine(ReturnToHubAfterDelay());

            IEnumerator ReturnToHubAfterDelay()
            {
                yield return new WaitForSeconds(delayBeforeReturnToHub);
                SceneController.LoadScene(0);
            }
        }

        private void InstantiateCamera()
        {
            var camera = Resources.Load<CinemachineVirtualCamera>(PlayerCameraPath);
            var cameraInstante = Instantiate(camera);
            cameraInstante.Follow = _player.transform;
        }

        private void InstantiateMobs()
        {
            _spawnPoints = GameObject.FindGameObjectsWithTag(MobSpawnPoint);
            _mobSpawner = new MobSpawner(this, _spawnPoints, _player);
        }

        private void LaunchWavesLoop()
        {
            _currentWaveIndex++;
            _mobSpawner.TriggerNewWaveAfterDelay(delayBetweenWaves, mobsInWaves[_currentWaveIndex]);
        }

        private void HandleLevelGoalAchieved()
        {
            SceneController.LoadScene(0);
        }

        public void HandleWaveCleaned()
        {
            if (_currentWaveIndex == mobsInWaves.Length - 1)
            {
                HandleLevelGoalAchieved();
                print("Goal achieved. Level ended");
                return;
            }

            LaunchWavesLoop();
        }
    }
}
