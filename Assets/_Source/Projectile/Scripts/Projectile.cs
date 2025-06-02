using PocketZoneTest;
using UnityEngine;

public class Projectile : MonoBehaviour, ILaunchable
{
    [SerializeField] private float _launchSpeed = 10f;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _maxDistance;

    private Vector2 _startPosition;
    private Vector2 _direction;
    private int _damage;
    private bool _isLaunched;

    public void Launch(Vector2 launchDirection, int damage)
    {
        _direction = launchDirection.normalized;
        _damage = damage;
        _isLaunched = true;
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (_isLaunched)
        {
            if (Vector2.Distance(_startPosition, transform.position) >= _maxDistance)
            {
                gameObject.SetActive(false);
            }
            transform.position += (Vector3)(_direction * _launchSpeed * Time.deltaTime);
            CheckForCollisions();
        }
    }

    private void CheckForCollisions()
    {
        Vector2 forwardRaycastOrigin = (Vector2)transform.position + _direction * _raycastDistance * 0.2f;
        RaycastHit2D forwardHit = Physics2D.Raycast(forwardRaycastOrigin, _direction, _raycastDistance, _enemyLayer);

        if (forwardHit.collider != null)
        {
            _isLaunched = false;
            if (forwardHit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.DealDamage(_damage);
            }
            gameObject.SetActive(false);
            return;
        }

        Vector2 backwardRaycastOrigin = (Vector2)transform.position - _direction * _raycastDistance * 0.2f;
        RaycastHit2D backwardHit = Physics2D.Raycast(backwardRaycastOrigin, -_direction, _raycastDistance, _enemyLayer);

        if (backwardHit.collider != null)
        {
            if (forwardHit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.DealDamage(_damage);
            }
            _isLaunched = false;
            gameObject.SetActive(false);
        }
    }
}
