using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PocketZoneTest
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        [SerializeField] private InventoryItem _itemPrefab;
        [SerializeField] private RectTransform _contentPanel;
        [SerializeField] private InventoryItemDescription _description;
        [SerializeField] private InventoryPointerFollow _inventoryPointerFollow;
        [SerializeField] private Button _deleteBtn;

        private List<InventoryItem> _listOfUIItems = new();

        private int _currentSelectedItemID = -1;

        private void Awake()
        {
            _inventoryPointerFollow.Toggle(false);
            _description.ResetDescription();
        }

        private void OnEnable()
        {
            _deleteBtn.onClick.AddListener(OnDeleteBtnClicked);
        }

        private void OnDisable()
        {
            _deleteBtn.onClick.RemoveListener(OnDeleteBtnClicked);
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryItem uiItem = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(_contentPanel);
                _listOfUIItems.Add(uiItem);
                uiItem.ItemClicked += OnItemClicked;
                uiItem.ItemBeginDrag += OnItemBeginDragged;
                uiItem.ItemEndDrag += OnItemEndDragged;
                uiItem.ItemDropped += OnItemDropped;
            }
        }

        public event Action<int> DescriptionRequested;
        public event Action<int> StartDragging;
        public event Action<int, int> ItemsSwapped;
        public event Action<int> ItemDeleteRequested;

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (_listOfUIItems.Count > itemIndex)
            {
                _listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            _description.SetDescription(itemImage, name, description);
            DeselectAllItems();
            _listOfUIItems[itemIndex].Select();
            _deleteBtn.gameObject.SetActive(true);
        }

        public void ResetSelection()
        {
            _deleteBtn.gameObject.SetActive(false);
            _description.ResetDescription();
            DeselectAllItems();
        }

        public void CreateDraggedItem(Sprite icon, int quantity)
        {
            _inventoryPointerFollow.Toggle(true);
            _inventoryPointerFollow.SetData(icon, quantity);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        private void ResetDraggedItem()
        {
            _inventoryPointerFollow.Toggle(false);
            _currentSelectedItemID = -1;
        }
        private void DeselectAllItems()
        {
            for (int i = 0; i < _listOfUIItems.Count; i++)
            {
                _listOfUIItems[i].Deselect();
            }
        }

        private void OnItemClicked(InventoryItem inventoryItem)
        {
            int index = _listOfUIItems.IndexOf(inventoryItem);

            if (index == -1)
                return;

            if (_currentSelectedItemID != -1)
            {
                _listOfUIItems[_currentSelectedItemID].Deselect();
            }
            _currentSelectedItemID = index;

            _listOfUIItems[_currentSelectedItemID].Select();
            _deleteBtn.gameObject.SetActive(false);
            DescriptionRequested?.Invoke(index);
        }

        private void OnItemBeginDragged(InventoryItem inventoryItem)
        {
            int index = _listOfUIItems.IndexOf(inventoryItem);

            if (index == -1)
                return;
            _currentSelectedItemID = index;
            OnItemBeginDragged(inventoryItem);
            StartDragging?.Invoke(index);
        }

        private void OnItemEndDragged(InventoryItem inventoryItem)
        {
            ResetDraggedItem();
        }

        public void ResetAllItems()
        {
            foreach (var item in _listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        private void OnItemDropped(InventoryItem inventoryItem)
        {
            int index = _listOfUIItems.IndexOf(inventoryItem);

            if (index == -1)
            {
                return;
            }
            ItemsSwapped?.Invoke(_currentSelectedItemID, index);
        }

        private void OnDeleteBtnClicked()
        {
            if (_currentSelectedItemID == -1)
                return;

            _deleteBtn.gameObject.SetActive(false);
            ResetSelection();
            ItemDeleteRequested?.Invoke(_currentSelectedItemID);
        }
    }
}
