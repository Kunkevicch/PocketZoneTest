using UnityEngine;

namespace PocketZoneTest
{
    public class EnemyStateDead : EnemyStateBase
    {
        private readonly CapsuleCollider2D _bodyCollider;
        public EnemyStateDead(Enemy enemy, bool needsExitTime) : base(enemy, needsExitTime)
        {
            _bodyCollider = enemy.GetComponent<CapsuleCollider2D>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("IsDead", true);
            _bodyCollider.enabled = false;
        }
    }
}
