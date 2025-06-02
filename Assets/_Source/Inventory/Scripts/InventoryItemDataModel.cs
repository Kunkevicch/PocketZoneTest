using System;

namespace PocketZoneTest
{
    [Serializable]
    public struct InventoryItemDataModel
    {
        public ItemData item;
        public int quantity;
        public bool IsEmpty => item == null;

        public InventoryItemDataModel ChangeQuantity(int newQuantity)
            => new InventoryItemDataModel { item = this.item, quantity = newQuantity };

        public static InventoryItemDataModel GetEmptyItem() => new InventoryItemDataModel { item = null, quantity = 0 };
    }
}
