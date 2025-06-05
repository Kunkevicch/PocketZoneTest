using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PocketZoneTest
{
    public class InventoryModel : IInventoryModel
    {
        private readonly IStorageService _storageService;
        private readonly AvailableItem _availableItem;

        private List<InventoryItemDataModel> _inventoryItems;

        private readonly int _size;

        private const string SAVE_DATA_PATH = "inventory";

        public InventoryModel(
            AvailableItem availableItem
            , IStorageService storageService
            , int size)
        {
            _availableItem = availableItem;
            _storageService = storageService;
            _size = size;
        }

        public event Action InventoryChanged;

        public int Size => _size;

        public void Initialize()
        {
            _inventoryItems = new List<InventoryItemDataModel>();
            for (int i = 0; i < _size; i++)
            {
                _inventoryItems.Add(InventoryItemDataModel.GetEmptyItem());
            }
        }

        public void LoadData()
        => _storageService.Load<SaveDataInventory>(SAVE_DATA_PATH, OnLoad);


        private void OnLoad(SaveDataInventory loadedData)
        {
            if (loadedData != null)
            {
                for (int i = 0; i < loadedData.InventoryItems.Count; i++)
                {
                    InventoryItemDataModel slotDataModel = _inventoryItems[i];
                    slotDataModel.item = GetAvailableItemByID(loadedData.InventoryItems[i].itemID);
                    slotDataModel = slotDataModel.ChangeQuantity(loadedData.InventoryItems[i].quantity);
                    _inventoryItems[i] = slotDataModel;
                }
            }

            InventoryChanged += OnInventoryChanged;
            InventoryChanged?.Invoke();
        }

        private void OnInventoryChanged()
        {
            SaveDataInventory saveData = new();
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                InventorySaveDataDetails saveDataDetails = new()
                {
                    quantity = _inventoryItems[i].quantity,
                    itemID = _inventoryItems[i].IsEmpty ? "" : _inventoryItems[i].item.ID,
                    IsEmpty = _inventoryItems[i].IsEmpty
                };
                saveData.InventoryItems.Add(i, saveDataDetails);
                _storageService.Save(SAVE_DATA_PATH, saveData);
            }
        }

        public int AddItem(ItemData item, int quantity)
        {
            if (!item.IsStackable)
            {
                for (int i = 0; i < _inventoryItems.Count; i++)
                {
                    while (quantity > 0 && !IsInventoryHasFreeSlot())
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1);
                    }
                    InventoryChanged?.Invoke();
                    return quantity;

                }
            }
            quantity = AddStackableItem(item, quantity);
            InventoryChanged?.Invoke();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemData item, int quantity)
        {
            InventoryItemDataModel newItemDataModel = new() { item = item, quantity = quantity };

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    _inventoryItems[i] = newItemDataModel;
                    return quantity;
                }
            }
            return 0;
        }

        public bool IsInventoryFull()
        => !_inventoryItems.Any(
               item => item.IsEmpty
               || !item.IsEmpty
               && item.quantity < item.item.MaxStackSize
               );

        private bool IsInventoryHasFreeSlot()
        => !_inventoryItems.Any(item => item.IsEmpty);

        private int AddStackableItem(ItemData item, int quantity)
        {
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                    continue;

                if (_inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake
                        = _inventoryItems[i].item.MaxStackSize - _inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake;

                    }
                    else
                    {
                        _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i].quantity + quantity);
                        InventoryChanged?.Invoke();
                        return 0;
                    }
                }
            }

            while (quantity > 0 && !IsInventoryHasFreeSlot())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItemDataModel item) => AddItem(item.item, item.quantity);

        public void DeleteItem(int itemID, int amount)
        {
            if (_inventoryItems.Count > itemID)
            {
                if (_inventoryItems[itemID].IsEmpty)
                    return;

                int reminder = _inventoryItems[itemID].quantity - amount;

                if (reminder <= 0)
                {
                    _inventoryItems[itemID] = InventoryItemDataModel.GetEmptyItem();
                }
                else
                {
                    _inventoryItems[itemID] = _inventoryItems[itemID].ChangeQuantity(reminder);
                }
                InventoryChanged?.Invoke();
            }
        }

        public void DeleteItem(InventoryItemDataModel item)
        {
            var findedItem = _inventoryItems.IndexOf(item);
            DeleteItem(findedItem, 1);
        }

        public void DeleteItem(int itemID, DeleteMode deleteMode)
        {
            switch (deleteMode)
            {
                case DeleteMode.Single:
                    DeleteItem(itemID, 1); 
                    break;

                case DeleteMode.Full:
                    DeleteItem(itemID, _inventoryItems[itemID].quantity);
                    break;

                default:
                    DeleteItem(itemID, 1);
                    break;
            }
        }

        public Dictionary<int, InventoryItemDataModel> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItemDataModel> returnValue =
                new();

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    continue;
                }
                returnValue[i] = _inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItemDataModel GetItemByID(int itemID)
        => _inventoryItems[itemID];

        public ItemData GetAvailableItemByID(string itemID)
        => _availableItem.Items.ToList().Find(x => x.ID == itemID);
        public void SwapItems(int firstItemID, int secondItemID)
        {
            (_inventoryItems[secondItemID], _inventoryItems[firstItemID])
                = (_inventoryItems[firstItemID], _inventoryItems[secondItemID]);
            InventoryChanged?.Invoke();
        }

        public int TryConsumeItemFromMultipleStacks(ItemData itemData, int requiredAmount)
        {
            int remainingAmount = requiredAmount;
            int consumedAmmo = 0;

            foreach (var item in _inventoryItems.ToList())
            {
                if (item.IsEmpty)
                    continue;
                if (item.item.ID == itemData.ID && item.quantity > 0)
                {
                    if (item.quantity >= remainingAmount)
                    {
                        DeleteItem(_inventoryItems.IndexOf(item), remainingAmount);
                        consumedAmmo += remainingAmount;
                        remainingAmount = 0;
                        break;
                    }
                    else
                    {
                        consumedAmmo += item.quantity;
                        remainingAmount -= item.quantity;
                        DeleteItem(_inventoryItems.IndexOf(item), item.quantity);
                    }
                }

                if (remainingAmount <= 0)
                {
                    break;
                }
            }

            if (consumedAmmo > 0)
            {
                InventoryChanged?.Invoke();
            }

            return consumedAmmo;
        }

        public int GetItemCount(ItemData item)
        {
            return _inventoryItems
                .Where(itemInInventory => !itemInInventory.IsEmpty
                && itemInInventory.item.ID == item.ID)
                .Sum(itemInInventory => itemInInventory.quantity);
        }


    }
}
