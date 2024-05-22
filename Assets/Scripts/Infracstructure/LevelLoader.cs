using SGJ.Player;
using UnityEngine;

namespace SGJ.Infrastructure
{
    public class LevelLoader : MonoBehaviour
    {
        private const string PlayerPrefabPath = "Prefabs/Player/Player";

        private void Start()
        {
            var player = Resources.Load<PlayerMovement>(PlayerPrefabPath);
            var playerInstance = Instantiate(player);

            var playerServices = playerInstance.GetComponents<IGameService>();
            foreach (var service in playerServices)
                service.OnServiceInstantiated();
        }
    }
}