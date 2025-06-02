using System;
using UnityEngine;

namespace PocketZoneTest
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _playerLayer;

        private Transform _target;

        public event Action PlayerDetected;
        public event Action PlayerLost;

        public Transform Target => _target;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.IsInLayer(_playerLayer))
            {
                _target = collision.transform;
                PlayerDetected?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.IsInLayer(_playerLayer))
            {
                _target = null;
                PlayerLost?.Invoke();
            }
        }
    }
}
