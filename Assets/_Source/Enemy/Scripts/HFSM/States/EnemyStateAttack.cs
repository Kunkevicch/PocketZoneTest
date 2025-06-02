using System;
using UnityHFSM;

namespace PocketZoneTest
{
    public class EnemyStateAttack : EnemyStateBase
    {
        public EnemyStateAttack(
            Enemy enemy
            , bool needsExitTime
            , float exitTime
            , Action<State<EnemyState, EnemyStateEvent>> onEnter
            , Func<State<EnemyState, EnemyStateEvent>, bool> canExit
            ) : base(
                enemy
                , needsExitTime
                , exitTime
                , onEnter
                , canExit: canExit
           )
        {
        }
    }
}
