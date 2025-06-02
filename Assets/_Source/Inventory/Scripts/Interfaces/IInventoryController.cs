using System;

namespace PocketZoneTest
{
    public interface IInventoryController
    {
        event Action InventoryChanged;
        void AddItem(ItemData item, int quantity);
        int GetItemCount(ItemData item);
        int TryConsumeItemFromMultipleStacks(ItemData item, int requiredAmount);
        bool IsInventoryFull();
    }
}