using UnityEngine;

namespace SGJ.Mobs 
{ 
    public class MobSpawner : MonoBehaviour
    {
        private const string MOBS_PATH = "Prefabs/Mobs";
        [SerializeField] private int mobsQuantity;

        private PlayerController _player;
        private Mob[] _mobs;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();
            _mobs = Resources.LoadAll<Mob>(MOBS_PATH);
            SpawnMobs();
        }

        private void SpawnMobs()
        {
            for (int i = 0; i < mobsQuantity; i++)
            {
                foreach (var mob in _mobs)
                {
                    var instance = Instantiate(mob, transform);
                    instance.SetMobParameters(_player.transform);
                }
            }
        }

        private void Update()
        {
            #if UNITY_EDITOR
            
            // Debug ONLY
            if (Input.GetKeyDown(KeyCode.Space))
                SpawnMobs();
            
            #endif
        }

    }
}
