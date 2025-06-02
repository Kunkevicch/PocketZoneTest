using UnityEngine;
using Zenject;

namespace PocketZoneTest
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private LayerMask _itemLayer;

        private IInventoryController _inventoryController;

        [Inject]
        public void Construct(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.IsInLayer(_itemLayer))
            {
                if (collision.TryGetComponent(out IPickupable ipickable))
                {
                    if (!_inventoryController.IsInventoryFull())
                    {
                        var item = ipickable.Pickup();
                        _inventoryController.AddItem(item, ipickable.Quantity);
                    }
                }
            }
        }
    }
}
