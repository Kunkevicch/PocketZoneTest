using System.Collections;
using UnityEngine;

namespace PocketZoneTest
{
    public class Pickable : MonoBehaviour, IPickupable
    {
        [SerializeField] private ItemData _ItemData;
        [SerializeField] private float _pickUpDuration;
        [SerializeField] private int _quantity;
        private CapsuleCollider2D _capsuleCollider;

        private bool _isPickable = true;

        private void Awake()
        {
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        public int Quantity => _quantity;

        public ItemData Pickup()
        {
            if (!_isPickable)
                return null;

            _isPickable = false;
            _capsuleCollider.enabled = false;

            StartCoroutine(DestroyRoutine());

            return _ItemData;
        }

        private IEnumerator DestroyRoutine()
        {
            Vector3 startScale = transform.localScale;
            float currentTime = 0;

            while (currentTime < _pickUpDuration)
            {
                currentTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, currentTime / _pickUpDuration);
                yield return null;
            }
            transform.localScale = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
