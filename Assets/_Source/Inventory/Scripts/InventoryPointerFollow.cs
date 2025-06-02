using UnityEngine;

namespace PocketZoneTest
{
    public class InventoryPointerFollow : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _mainCam;

        private InventoryItem _item;

        private void Awake() => _item = GetComponentInChildren<InventoryItem>();


        public void SetData(Sprite icon, int quantity) => _item.SetData(icon, quantity);


        private void Update()
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_canvas.transform,
                Input.mousePosition,
                _canvas.worldCamera,
                out position
                );

            transform.position = _canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool visibility) => gameObject.SetActive(visibility);

    }
}
