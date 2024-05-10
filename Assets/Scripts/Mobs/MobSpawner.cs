using SGJ.Player;
using SGJ.SceneManagement;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace SGJ.Mobs
{
    public class MobSpawner
    {
        private const string MobsPath = "Prefabs/Mobs";
        private const int MobsPerWave = 20;

        private readonly GoalObserver _goalObserver;
        private readonly GameObject[] _spawnPoints;
        private readonly PlayerController _player;
        private readonly Mob[] _mobs;

        [SerializeField] private int mobsQuantity;
        private int _mobToKillLeft;

        private Mob GetRandomMob => _mobs[Random.Range(0, _mobs.Length)];
        private Transform GetRandomSpawnPoint => _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;

        public MobSpawner(GoalObserver goalObserver, GameObject[] spawnPoints, PlayerController player)
        {
            _goalObserver = goalObserver;
            _spawnPoints = spawnPoints;
            _player = player;
            _mobs = Resources.LoadAll<Mob>(MobsPath);
        }

        internal void HandleMobDeath(object sender, Mob deadMob)
        {
            deadMob.MobDropper.TryDropItem();
            Object.Destroy(deadMob.gameObject);
            _mobToKillLeft--;
            Debug.Log($"{_mobToKillLeft} left");
            if (_mobToKillLeft == 0)
                _goalObserver.HandleWaveCleaned();
        }

        internal void TriggerNewWaveAfterDelay(float delayBetweenWaves)
        {
            _goalObserver.StartCoroutine(StartNewWaveAfterDelay());

            IEnumerator StartNewWaveAfterDelay()
            {
                yield return new WaitForSeconds(delayBetweenWaves);
                StartNewWave();
            }

            void StartNewWave()
            {
                for (int i = 0; i < MobsPerWave; i++)
                {
                    var instance = Object.Instantiate(GetRandomMob, GetRandomSpawnPoint);
                    instance.SetMobParameters(_player, this);
                }
                _mobToKillLeft = MobsPerWave;
            }
        }
    }
}
