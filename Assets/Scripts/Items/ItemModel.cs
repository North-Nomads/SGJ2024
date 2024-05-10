using UnityEngine;

namespace SGJ.GameItems
{
    public enum Items
    {
        Ammo,
        Medkit,
        Sonar,
        LifeAnalizer,
        Grenade,
        Dynamite
    }

    [CreateAssetMenu(menuName = "Items/Item")]
    public class ItemModel : ScriptableObject
    {
        [SerializeField] private Items item;
        [SerializeField] private DroppedItem prefab;

        public Items Item => item;
        public DroppedItem Prefab => prefab;
    }
}