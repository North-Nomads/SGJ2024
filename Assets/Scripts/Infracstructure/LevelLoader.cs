using SGJ.Player;
using SGJ.Weapons.Instances;
using UnityEngine;

namespace SGJ.Infrastructure
{
    public class LevelLoader : MonoBehaviour
    {
        private const string PlayerPrefabPath = "Prefabs/Player/Player";
        private const string MinigunPrefabPath = "Prefabs/Weapons/Minigun";

        private void Start()
        {
            InstantiatePlayer();

            // TODO: rework according to current save
            EquipMinigun();
        }

        private static void InstantiatePlayer()
        {
            var player = Resources.Load<PlayerMovement>(PlayerPrefabPath);
            var playerInstance = Instantiate(player);

            var playerServices = playerInstance.GetComponents<IGameService>();
            foreach (var service in playerServices)
                service.OnServiceInstantiated();
        }

        private static void EquipMinigun()
        {
            var minigunPrefab = Resources.Load<Minigun>(MinigunPrefabPath);
            PlayerWeaponry playerWeaponry = ServiceLocator.Current.Get<PlayerWeaponry>();
            var minigunInstance = Instantiate(minigunPrefab, playerWeaponry.WeaponRootPosition);
            playerWeaponry.EquipWeapon(minigunInstance);
        }
    }
}