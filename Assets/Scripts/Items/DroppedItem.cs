using SGJ.Player;
using UnityEngine;

namespace SGJ.GameItems
{
    [RequireComponent(typeof(BoxCollider))]
    public class DroppedItem : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        private Items _item;

        public void OnObjectCreated(Items item) => _item = item;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                other.GetComponent<PlayerInventory>().AddItemOfType(_item);
                Destroy(gameObject);
            }
                
        }
    }
}