using UnityEngine;

namespace PocketZoneTest
{
    public class EnemyStateMove : EnemyStateBase
    {
        private readonly Transform _visual;
        private readonly PlayerDetector _detector;
        private readonly float _moveSpeed;

        private readonly float _xTolerance;
        private readonly float _yTolerance;

        public EnemyStateMove(
           Enemy enemy
           , bool needsExitTime
           , Transform visual
           , PlayerDetector detector
           , float moveSpeed
           , float xTolerance
           , float yTolerance
           ) : base(
               enemy
               , needsExitTime
               )
        {
            _visual = visual;
            _detector = detector;
            _moveSpeed = moveSpeed;
            _xTolerance = xTolerance;
            _yTolerance = yTolerance;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("IsMove", true);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (!_requestedExit)
            {
                MoveTowardsTarget();
            }
            else if (_rb2D.position.x <= _xTolerance && _rb2D.position.y <= _yTolerance)
            {
                fsm.StateCanExit();
            }
        }
        private void MoveTowardsTarget()
        {
            Vector2 enemyPosition = _rb2D.position;
            Vector2 targetPosition = _detector.Target.position;

            if (Mathf.Abs(enemyPosition.y - targetPosition.y) > _yTolerance)
            {
                enemyPosition.y = Mathf.Lerp(enemyPosition.y, targetPosition.y, Time.deltaTime * _moveSpeed);
            }

            float directionX = targetPosition.x - enemyPosition.x;

            if (directionX < 0)
            {
                _visual.localEulerAngles = new Vector3(0, 180f, 0);
            }
            else
            {
                _visual.localEulerAngles = Vector3.zero;
            }

            if (Mathf.Abs(directionX) > _xTolerance)
            {
                enemyPosition.x += directionX * Time.deltaTime * _moveSpeed;
            }

            _rb2D.MovePosition(enemyPosition);
        }
    }
}
