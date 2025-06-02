using System;
using UnityEngine;

namespace PocketZoneTest
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Configs/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField, HideInInspector] private string _id;

        public string ID => _id;

        [field: SerializeField] public bool IsStackable { get; private set; }
        [field: SerializeField] public int MaxStackSize { get; private set; }
        [field: SerializeField] public string Name { get; private set; }

        [field: TextArea]
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }
        }
#endif
    }
}
