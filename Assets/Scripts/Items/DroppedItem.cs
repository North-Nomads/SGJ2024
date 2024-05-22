using SGJ.Player;
using SGJ.Proprs;
using UnityEngine;

namespace SGJ.GameItems
{
    [RequireComponent(typeof(BoxCollider))]
    public class DroppedItem : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        private Items _item;
        private int _quantity;
        
        public Items Item => _item;
        public int Quantity => _quantity;

        public void OnObjectCreated(Items item, int quantity=1)
        {
            _item = item;
            _quantity = quantity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                var pickers = other.GetComponents<IPickerUp>();
                foreach (var picker in pickers)
                    picker.AddItemsInInventory(this);
                
                Destroy(gameObject);
            }
        }
    }
}