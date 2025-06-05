using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PocketZoneTest
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionLayer;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Transform _visual;
        [SerializeField] private Transform _shoulderTransform;

        private readonly List<Enemy> _enemiesInSight = new();

        private Rigidbody2D _rb2D;
        private Health _health;

        private PlayerAnimator _animator;
        private PlayerCombat _combat;
        private Mover _mover;

        private bool _isActive = true;

        private IInput _input;
        private IEventMediator _eventMediator;

        [Inject]
        public void Construct(
            IInput input
            , IEventMediator eventMediator
            )
        {
            _input = input;
            _eventMediator = eventMediator;
        }

        private bool IsEnemyInSight => _enemiesInSight.Count != 0;

        private void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();

            _mover = new(_rb2D, _collisionLayer, transform);
            _animator = new(GetComponentInChildren<Animator>());
            _combat = GetComponent<PlayerCombat>();
            _combat.SetWeapon(GetComponentInChildren<IWeapon>());
        }

        private void OnEnable()
        {
            _isActive = true;

            _input.Fire += OnFire;
            _health.Dead += OnDead;

            _eventMediator.AddListener<EnemyInSightEvent>(OnEnemyInSight);
            _eventMediator.AddListener<EnemyOutSightEvent>(OnEnemyOutSight);
        }

        private void OnDisable()
        {
            _input.Fire -= OnFire;
            _health.Dead -= OnDead;

            _eventMediator.RemoveListener<EnemyInSightEvent>(OnEnemyInSight);
            _eventMediator.RemoveListener<EnemyOutSightEvent>(OnEnemyOutSight);
        }

        private void Update()
        {
            MoveToDirection(_input.InputDirection);
        }

        private void MoveToDirection(Vector2 direction)
        {
            if (!_isActive)
                return;

            if (direction != Vector2.zero)
            {
                if (!IsEnemyInSight)
                {
                    RotateToDirection(direction);
                    RotateShoulderToDirection(direction);
                }

                _animator.PlayMove();
            }
            else
            {

                _animator.PlayIdle();
            }

            if (IsEnemyInSight)
            {
                Vector3 directionToClosestEnemy = GetDirectionToClosestEnemy();
                RotateToDirection(directionToClosestEnemy);
                RotateShoulderToDirection(directionToClosestEnemy);
            }

            _mover.MoveProcess(direction, _moveSpeed);
        }

        private void RotateToDirection(Vector2 direction)
        {
            if (direction.x < 0)
            {
                _visual.localEulerAngles = new Vector3(0, 180f, 0f);
            }
            else
            {
                _visual.localEulerAngles = Vector3.zero;
            }
        }

        private void RotateShoulderToDirection(Vector2 direction)
        {
            bool isFacingLeft = _visual.localEulerAngles.y > 90f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (isFacingLeft)
            {
                angle = 180f - angle;
            }

            _shoulderTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        private void OnFire()
        {
            if (!_isActive)
                return;

            _combat.Attack();
        }

        private void OnDead()
        {
            if (!_isActive)
                return;

            _animator.PlayDead();
            _isActive = false;
        }

        private void OnEnemyInSight(EnemyInSightEvent enemyInSightEvent)
        {
            if (!_enemiesInSight.Contains(enemyInSightEvent.Enemy))
            {
                _enemiesInSight.Add(enemyInSightEvent.Enemy);
            }
        }

        private void OnEnemyOutSight(EnemyOutSightEvent enemyOutSightEvent)
        {
            if (_enemiesInSight.Contains(enemyOutSightEvent.Enemy))
            {
                _enemiesInSight.Remove(enemyOutSightEvent.Enemy);
            }

            if (_enemiesInSight.Count == 0)
            {
                _eventMediator.SendMessage(new AllEnemiesOutSightEvent());
            }
        }

        private Vector2 GetDirectionToClosestEnemy()
        {
            float closestDistanceSqr = Mathf.Infinity;
            Vector2 directionToEnemy = Vector2.zero;
            foreach (Enemy enemy in _enemiesInSight)
            {
                if (enemy == null)
                    continue;

                directionToEnemy = enemy.transform.position - transform.position;
                float distanceSqr = directionToEnemy.sqrMagnitude;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                }
            }

            return directionToEnemy.normalized;
        }
    }
}
