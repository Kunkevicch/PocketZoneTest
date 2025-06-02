using System;
using UnityEngine;
using UnityHFSM;

namespace PocketZoneTest
{
    public class EnemyStateBase : State<EnemyState, EnemyStateEvent>
    {
        protected readonly Enemy _enemy;
        protected readonly Rigidbody2D _rb2D;
        protected readonly Animator _animator;
        protected bool _requestedExit;
        protected float _exitTime;

        protected readonly Action<State<EnemyState, EnemyStateEvent>> _onEnter;
        protected readonly Action<State<EnemyState, EnemyStateEvent>> _onLogic;
        protected readonly Action<State<EnemyState, EnemyStateEvent>> _OnExit;
        protected readonly Func<State<EnemyState, EnemyStateEvent>, bool> _canExit;

        public EnemyStateBase(
            Enemy enemy
            , bool needsExitTime
            , float exitTime = 0.1f
            , Action<State<EnemyState, EnemyStateEvent>> onEnter = null
            , Action<State<EnemyState, EnemyStateEvent>> onLogic = null
            , Action<State<EnemyState, EnemyStateEvent>> onExit = null
            , Func<State<EnemyState, EnemyStateEvent>, bool> canExit = null
            ) : base(needsExitTime: needsExitTime)
        {

            _enemy = enemy;
            _exitTime = exitTime;
            _onEnter = onEnter;
            _onLogic = onLogic;
            _OnExit = onExit;
            _canExit = canExit;
            _rb2D = _enemy.GetComponent<Rigidbody2D>();
            _animator = enemy.GetComponentInChildren<Animator>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _requestedExit = false;
            _onEnter?.Invoke(this);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (_requestedExit && timer.Elapsed >= _exitTime)
            {
                fsm.StateCanExit();
            }
        }

        public override void OnExitRequest()
        {
            if (!needsExitTime || _canExit != null && _canExit(this))
            {
                fsm.StateCanExit();
            }
            else
            {
                _requestedExit = true;
            }
        }
    }
}
