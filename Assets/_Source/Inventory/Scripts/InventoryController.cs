using System;
using Zenject;

namespace PocketZoneTest
{
    public class InventoryController : IInitializable, IDisposable, IInventoryController
    {
        private readonly IInventoryView _inventoryView;
        private readonly IInventoryModel _inventoryModel;
        private readonly IInput _input;
        private readonly int _inventorySize;

        private bool _isInventoryOpened;

        public InventoryController(
             int inventorySize
            , IInventoryView inventoryView
            , IInventoryModel inventoryModel
            , IInput input
            )
        {
            _inventoryView = inventoryView;
            _inventoryModel = inventoryModel;
            _input = input;
            _inventorySize = inventorySize;
        }

        public event Action InventoryChanged;

        public void Initialize()
        {
            Subcribe();
            _inventoryModel.LoadData();
        }

        public void Dispose()
        {
            Unsubcribe();
        }

        private void Subcribe()
        {
            _inventoryModel.InventoryChanged += OnInventoryChanged;

            _inventoryModel.Initialize();
            _inventoryView.InitializeInventoryUI(_inventorySize);

            _inventoryView.DescriptionRequested += OnDescriptionRequested;
            _inventoryView.ItemsSwapped += OnItemSwapped;
            _inventoryView.StartDragging += OnStartDragging;
            _inventoryView.ItemDeleteRequested += OnItemDeleteRequested;

            _input.SwitchInventoryState += OnSwitchInventoryState;
        }

        private void Unsubcribe()
        {
            _inventoryModel.InventoryChanged -= OnInventoryChanged;

            _inventoryView.DescriptionRequested -= OnDescriptionRequested;
            _inventoryView.ItemsSwapped -= OnItemSwapped;
            _inventoryView.StartDragging -= OnStartDragging;
            _inventoryView.ItemDeleteRequested -= OnItemDeleteRequested;

            _input.SwitchInventoryState -= OnSwitchInventoryState;
        }

        public void AddItem(ItemData item, int quantity)
        {
            if (item != null || quantity != 0)
            {
                _inventoryModel.AddItem(item, quantity);
            }
        }

        private void OnInventoryChanged()
        {
            _inventoryView.ResetAllItems();
            var currentInventoryState = _inventoryModel.GetCurrentInventoryState();

            foreach (var keyPair in currentInventoryState)
            {
                _inventoryView.UpdateData(keyPair.Key, keyPair.Value.item.Image, keyPair.Value.quantity);
            }

            InventoryChanged?.Invoke();
        }

        private void OnDescriptionRequested(int itemID)
        {
            InventoryItemDataModel itemDataModel = _inventoryModel.GetItemByID(itemID);
            if (itemDataModel.IsEmpty)
            {
                _inventoryView.ResetSelection();
                return;
            }

            ItemData item = itemDataModel.item;
            _inventoryView.UpdateDescription(itemID, item.Image, item.Name, item.Description);
        }

        private void OnStartDragging(int itemID)
        {
            InventoryItemDataModel inventoryItemDataModel = _inventoryModel.GetItemByID(itemID);
            if (inventoryItemDataModel.IsEmpty)
                return;

            _inventoryView.CreateDraggedItem(inventoryItemDataModel.item.Image, inventoryItemDataModel.quantity);
        }

        private void OnItemSwapped(int firstItemID, int secondItemID) 
        => _inventoryModel.SwapItems(firstItemID, secondItemID);

        private void OnItemDeleteRequested(int itemID) 
        => _inventoryModel.DeleteItem(itemID, DeleteMode.Full);

        private void OnSwitchInventoryState()
        {
            if (!_isInventoryOpened)
            {
                _inventoryView.Show();
            }
            else
            {
                _inventoryView.Hide();
            }
            _isInventoryOpened = !_isInventoryOpened;
        }

        public int GetItemCount(ItemData item)
        => _inventoryModel.GetItemCount(item);

        public int TryConsumeItemFromMultipleStacks(ItemData item, int requiredAmount)
        => _inventoryModel.TryConsumeItemFromMultipleStacks(item, requiredAmount);

        public bool IsInventoryFull()
        => _inventoryModel.IsInventoryFull();
    }
}
