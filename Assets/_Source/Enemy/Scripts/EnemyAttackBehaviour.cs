using System;
using UnityEngine;

namespace PocketZoneTest
{
    public class EnemyAttackBehaviour : StateMachineBehaviour
    {
        public event Action AttackEnded;

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AttackEnded?.Invoke();
        }
    }
}
