using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PocketZoneTest
{
    public class InventoryItemDescription : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;

        public void ResetDescription()
        {
            _icon.gameObject.SetActive(false);
            _titleText.text = "";
            _descriptionText.text = "";
        }

        public void SetDescription(Sprite icon, string itemName, string itemDescription)
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = icon;
            _titleText.text = itemName;
            _descriptionText.text = itemDescription;
        }
    }
}
