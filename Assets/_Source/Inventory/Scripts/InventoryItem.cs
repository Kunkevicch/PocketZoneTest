using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PocketZoneTest
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private Image _borderImage;

        public event Action<InventoryItem> ItemClicked;
        public event Action<InventoryItem> ItemDropped;
        public event Action<InventoryItem> ItemBeginDrag;
        public event Action<InventoryItem> ItemEndDrag;

        private bool _isEmpty = true;

        private void Awake()
        {
            ResetData();
            Deselect();
        }


        public void ResetData()
        {
            _itemImage.gameObject.SetActive(false);
            _isEmpty = true;
        }

        public void Select() => _borderImage.gameObject.SetActive(true);

        public void Deselect() => _borderImage.gameObject.SetActive(false);


        public void SetData(Sprite sprite, int quantity)
        {
            _itemImage.gameObject.SetActive(true);
            _itemImage.sprite = sprite;
            _quantityText.text = quantity.ToString();
            _isEmpty = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isEmpty)
                return;

            ItemBeginDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemDropped?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ItemEndDrag?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ItemClicked?.Invoke(this);
        }
    }
}
