using UnityEngine;

namespace PocketZoneTest
{
    [CreateAssetMenu(fileName = "AvailableItems_", menuName = "Configs/Avaliable Items")]
    public class AvailableItem : ScriptableObject
    {
        [field: SerializeField] public ItemData[] Items { get; private set; }
    }
}
