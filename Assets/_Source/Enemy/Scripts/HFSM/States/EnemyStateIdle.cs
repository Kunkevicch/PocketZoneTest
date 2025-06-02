using System;
using UnityHFSM;

namespace PocketZoneTest
{
    public class EnemyStateIdle : EnemyStateBase
    {
        public EnemyStateIdle(Enemy enemy, bool needsExitTime) : base(enemy, needsExitTime) { }
        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("IsMove", false);
        }

    }
}
