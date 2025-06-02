using System;
using UnityEngine;

namespace PocketZoneTest
{
    public interface IInventoryView
    {
        event Action<int> DescriptionRequested;
        event Action<int> ItemDeleteRequested;
        event Action<int, int> ItemsSwapped;
        event Action<int> StartDragging;

        void CreateDraggedItem(Sprite icon, int quantity);
        void Hide();
        void InitializeInventoryUI(int inventorySize);
        void ResetAllItems();
        void ResetSelection();
        void Show();
        void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity);
        void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description);
    }
}