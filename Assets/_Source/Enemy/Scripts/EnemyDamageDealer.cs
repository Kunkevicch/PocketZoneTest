using UnityEngine;

namespace PocketZoneTest
{
    public class EnemyDamageDealer : MonoBehaviour
    {
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private int _damage;
        [SerializeField] private Transform _damagePoint;
        [SerializeField] private float _damageRadius;

        public void DealDamage()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_damagePoint.position, _damageRadius, _playerLayer);
            
            if (hitColliders.Length > 0)
            {
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.DealDamage(_damage);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_damagePoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_damagePoint.position, _damageRadius);
            }
        }
    }
}