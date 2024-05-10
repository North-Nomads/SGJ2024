
using Cinemachine;
using SGJ.Mobs;
using SGJ.Player;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SGJ.SceneManagement
{
    public class AssetSpawner
    {
        private const string PlayerSpawnPoint = "PlayerSpawnPoint";
        private const string PlayerPrefabPath = "Prefabs/Player/Player";
        private const string PlayerCameraPath = "Prefabs/Player/Virtual Camera";
        
        private readonly GoalObserver _observer;
        private PlayerController _player;

        public PlayerController Player => _player;

        public AssetSpawner(GoalObserver observer)
        {
            _observer = observer;
            InstantiatePlayer();
            InstantiateCamera();
        }

        private void InstantiatePlayer()
        {
            var player = Resources.Load<PlayerController>(PlayerPrefabPath);
            var playerSpawnPoint = GameObject.FindGameObjectWithTag(PlayerSpawnPoint);
            if (playerSpawnPoint == null)
                throw new Exception($"No player spawn point found. Assign spawn point object corresponding Tag: {PlayerSpawnPoint}");
            _player = Object.Instantiate(player, playerSpawnPoint.transform.position, Quaternion.identity);
            _player.OnPlayerDied += _observer.HandlePlayerDied;
        }

        private void InstantiateCamera()
        {
            var camera = Resources.Load<CinemachineVirtualCamera>(PlayerCameraPath);
            var cameraInstante = Object.Instantiate(camera);
            cameraInstante.Follow = _player.transform;
        }
    }
}
