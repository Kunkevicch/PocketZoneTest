using UnityEngine;

namespace PocketZoneTest
{
    public class LootDropper : MonoBehaviour
    {
        [SerializeField] private Pickable _droppedItem;

        public void DropLoot()
        {
            Instantiate(_droppedItem.gameObject).transform.position = transform.position;
        }
    }
}
