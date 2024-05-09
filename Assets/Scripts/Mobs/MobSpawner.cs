using SGJ.Combat;
using SGJ.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ.Mobs 
{ 
    public class MobSpawner
    {
        private const string MobsPath = "Prefabs/Mobs";
        [SerializeField] private int mobsQuantity;

        private GoalObserver _goalObserver;
        private GameObject[] _spawnPoints;
        private PlayerMovement _player;
        private Mob[] _mobs;
        private int _mobToKillLeft;
        private List<Mob> _aliveMobs;

        private Mob GetRandomMob => _mobs[Random.Range(0, _mobs.Length)];
        private Transform GetRandomSpawnPoint => _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;

        public MobSpawner(GoalObserver goalObserver, GameObject[] spawnPoints, PlayerMovement player)
        {
            _goalObserver = goalObserver;
            _spawnPoints = spawnPoints;
            _player = player;
            _mobs = Resources.LoadAll<Mob>(MobsPath);
            Debug.Log($"Loaded {_mobs.Length} mobs");
        }

        private void StartNewWave(int mobsToSpawn)
        {
            Debug.Log($"Spawning {mobsToSpawn}");
            _aliveMobs = new List<Mob>(mobsToSpawn);
            _mobToKillLeft = mobsToSpawn;
            for (int i = 0; i < mobsToSpawn; i++)
            {
                var instance = Object.Instantiate(GetRandomMob, GetRandomSpawnPoint);
                instance.SetMobParameters(_player.transform, this);
                _aliveMobs.Add(instance);
            }
        }

        internal void HandleMobDeath(object sender, Mob deadMob)
        {
            Object.Destroy(deadMob.gameObject);
            _mobToKillLeft--;
            if (_mobToKillLeft == 0)
                _goalObserver.HandleWaveCleaned();
        }

        internal void TriggerNewWaveAfterDelay(float delayBetweenWaves, int mobsToSpawn)
        {
            _goalObserver.StartCoroutine(StartNewWaveAfterDelay());

            IEnumerator StartNewWaveAfterDelay()
            {
                yield return new WaitForSeconds(delayBetweenWaves);
                StartNewWave(mobsToSpawn);
            }
        }

        internal void KillAllMobs()
        {
            foreach (var instance in _aliveMobs)
            {
                (instance as IHittable).OnEntityGotHit(float.PositiveInfinity);
            }
        }
    }
}
