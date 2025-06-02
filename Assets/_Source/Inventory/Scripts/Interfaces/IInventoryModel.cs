using System;
using System.Collections.Generic;

namespace PocketZoneTest
{
    public interface IInventoryModel
    {
        int Size { get; }

        event Action InventoryChanged;

        void AddItem(InventoryItemDataModel item);
        int AddItem(ItemData item, int quantity);
        void DeleteItem(int itemID, int amount);
        void DeleteItem(InventoryItemDataModel item);
        Dictionary<int, InventoryItemDataModel> GetCurrentInventoryState();
        InventoryItemDataModel GetItemByID(int itemID);
        void Initialize();
        void LoadData();
        void SwapItems(int firstItemID, int secondItemID);
        int TryConsumeItemFromMultipleStacks(ItemData ammoData, int requiredAmount);
        int GetItemCount(ItemData item);
        bool IsInventoryFull();
    }
}