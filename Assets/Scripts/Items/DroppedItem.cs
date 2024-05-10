using SGJ.Player;
using UnityEngine;

namespace SGJ.GameItems
{
    [RequireComponent(typeof(BoxCollider))]
    public class DroppedItem : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        private Items _item;
        private int _quantity;

        public void OnObjectCreated(Items item, int quantity=1)
        {
            _item = item;
            _quantity = quantity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                //print($"{_item}, {_quantity}");
                other.GetComponent<PlayerController>().PlayerInventory[_item] += _quantity;
                Destroy(gameObject);
            }
                
        }
    }
}