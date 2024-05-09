using SGJ.Mobs;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace SGJ.SceneManagement
{
    public class GoalObserver : MonoBehaviour
    {
        [SerializeField] private int wavesQuantity;
        [SerializeField] private int[] mobsInWaves;
        [SerializeField] private float delayBetweenWaves;

        private const string MobSpawnPoint = "MobSpawnPoint";
        private const string PlayerSpawnPoint = "PlayerSpawnPoint";
        private const string PlayerPrefabPath = "Prefabs/Player/Player";
        private MobSpawner _mobSpawner;
        private GameObject[] _spawnPoints;
        private PlayerMovement _player;
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
            InstantiateMobs();
            LaunchWavesLoop();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _mobSpawner.KillAllMobs();
            }
        }

        private void InstantiatePlayer()
        {
            var player = Resources.Load<PlayerMovement>(PlayerPrefabPath);
            var playerSpawnPoint = GameObject.FindGameObjectWithTag(PlayerSpawnPoint);
            if (playerSpawnPoint == null)
                throw new Exception($"No player spawn point found. Assign spawn point object corresponding Tag: {PlayerSpawnPoint}");
            _player = Instantiate(player, playerSpawnPoint.transform.position, Quaternion.identity);
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
            throw new NotImplementedException();
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
