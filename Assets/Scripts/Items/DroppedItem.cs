using UnityEngine;

namespace SGJ.Player
{
    [RequireComponent(typeof(BoxCollider))]
    public class DroppedItem : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        [SerializeField] private Items itemType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
                other.GetComponent<PlayerInventory>().AddItemOfType(itemType);
        }
    }
}