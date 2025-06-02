using UnityEngine;

namespace PocketZoneTest
{
    public class PlayerAnimator
    {
        private readonly Animator _animator;

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void PlayIdle()
        {
            _animator.SetBool("isMove",false);
        }

        public void PlayMove()
        {
            _animator.SetBool("isMove", true);
        }

        public void PlayAttack()
        {
            _animator.SetBool("isAttack",false);
        }

        public void PlayDead()
        {
            _animator.SetBool("isDead",true);
        }
    }
}
