using UnityEngine;

namespace PocketZoneTest
{
    public class Mover
    {
        private readonly Rigidbody2D _rb2D;
        private readonly LayerMask _collisionLayer;
        private readonly Transform _transform;

        public Mover(Rigidbody2D rb2D, LayerMask collisionLayer, Transform transform)
        {
            _rb2D = rb2D;
            _collisionLayer = collisionLayer;
            _transform = transform;
        }

        public void MoveProcess(Vector2 direction, float moveSpeed)
        {
            RaycastHit2D hit = Physics2D.Raycast(_transform.position, direction, 0.1f, _collisionLayer);

            if (hit.collider == null)
            {
                _rb2D.MovePosition(_rb2D.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
