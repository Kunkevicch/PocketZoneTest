using UnityEngine;
using UnityHFSM;
using Zenject;

namespace PocketZoneTest
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _attackCoolDown;

        [SerializeField] private float _xTolerance;
        [SerializeField] private float _yTolerance;

        [SerializeField] private Transform _visual;

        [SerializeField] private PlayerDetector _playerDetector;
        [SerializeField] private PlayerDetector _attackZoneDetector;

        private bool _isDead;
        private bool _canAttack;
        private bool _isPlayerInRange;
        private bool _isAttack;

        private float _lastAttackTime;

        private Health _health;
        private Animator _animator;
        private EnemyAttackBehaviour _attackBehaviour;
        private LootDropper _lootDropper;

        private StateMachine<EnemyState, EnemyStateEvent> _fsm;

        private Transform _target;

        private IEventMediator _eventMediator;

        [Inject]
        public void Construct(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        private void Awake()
        {
            _health = GetComponent<Health>();
            _animator = GetComponentInChildren<Animator>();
            _lootDropper = GetComponent<LootDropper>();
            _attackBehaviour = _animator.GetBehaviour<EnemyAttackBehaviour>();
            InitializeFSM();
        }

        private void OnEnable()
        {
            _playerDetector.PlayerDetected += OnPlayerDetected;
            _playerDetector.PlayerLost += OnPlayerLost;

            _attackZoneDetector.PlayerDetected += OnPlayerInAttackRange;
            _attackZoneDetector.PlayerLost += OnPlayerOutAttackRange;

            _attackBehaviour.AttackEnded += OnAttackEnded;

            _health.Dead += OnDead;
        }

        private void OnDisable()
        {
            _playerDetector.PlayerDetected -= OnPlayerDetected;
            _playerDetector.PlayerLost -= OnPlayerLost;

            _attackZoneDetector.PlayerDetected -= OnPlayerInAttackRange;
            _attackZoneDetector.PlayerLost -= OnPlayerOutAttackRange;

            _attackBehaviour.AttackEnded += OnAttackEnded;

            _health.Dead -= OnDead;
        }

        private void Update()
        {
            _fsm.OnLogic();
        }

        private void OnBecameVisible()
        {
            if (_isDead)
                return;
            _eventMediator.SendMessage(new EnemyInSightEvent(this));
        }

        private void OnBecameInvisible()
        {
            if (_isDead)
                return;

            _eventMediator.SendMessage(new EnemyOutSightEvent(this));
        }

        private void InitializeFSM()
        {
            _fsm = new();

            _fsm.AddState(EnemyState.Idle, new EnemyStateIdle(this, false));
            _fsm.AddState(EnemyState.Move, new EnemyStateMove(this, false, _visual, _playerDetector, _moveSpeed, _xTolerance, _yTolerance));
            _fsm.AddState(EnemyState.Attack, new EnemyStateAttack(this, true, 0.7f, OnAttack, canExit: IsAttackOver));
            _fsm.AddState(EnemyState.Dead, new EnemyStateDead(this, false));

            _fsm.AddTriggerTransition(EnemyStateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Move));
            _fsm.AddTriggerTransition(EnemyStateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Move, EnemyState.Idle));

            _fsm.AddTransition(new Transition<EnemyState>(
                EnemyState.Idle
                , EnemyState.Move
                , condition: (transition)
                => _isPlayerInRange && _target != null &&
                Vector2.Distance(_target.position, transform.position) > _xTolerance));

            _fsm.AddTransition(new Transition<EnemyState>(
                EnemyState.Move
                , EnemyState.Idle
                , condition: (transition)
                => !_isPlayerInRange &&
                Vector2.Distance(_target.position, transform.position) <= _xTolerance));

            _fsm.AddTransition(new Transition<EnemyState>(EnemyState.Move, EnemyState.Attack, ShouldAttack));
            _fsm.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldAttack));

            _fsm.AddTransition(new Transition<EnemyState>(
                EnemyState.Attack
                , EnemyState.Idle
                , IsWithinIdleRange
                ));

            _fsm.AddTransition(new Transition<EnemyState>(
                EnemyState.Attack
                , EnemyState.Move
                , condition: (transition)
                => IsNotWithinIdleRange(transition) && _isAttack == false
                ));

            _fsm.AddTransitionFromAny(new Transition<EnemyState>(EnemyState.Dead, EnemyState.Dead, (transition) => _isDead));

            _fsm.SetStartState(EnemyState.Idle);
            _fsm.Init();
        }

        private void OnAttack(State<EnemyState, EnemyStateEvent> obj)
        {
            _isAttack = true;
            _lastAttackTime = Time.time;
            _animator.SetTrigger("Attack");
        }

        private void OnAttackEnded()
        {
            _isAttack = false;
        }

        private bool ShouldAttack(Transition<EnemyState> transition) =>
            _lastAttackTime + _attackCoolDown <= Time.time
            && _canAttack;

        private void OnPlayerDetected()
        {
            _isPlayerInRange = true;
            _fsm.Trigger(EnemyStateEvent.DetectPlayer);
        }

        private void OnPlayerLost()
        {
            _isPlayerInRange = false;
            _target = null;
            _fsm.Trigger(EnemyStateEvent.LostPlayer);
        }

        private void OnPlayerInAttackRange()
        {
            _canAttack = true;
        }
        private void OnPlayerOutAttackRange()
        {
            _canAttack = false;
        }

        private void OnDead()
        {
            _isDead = true;
            _eventMediator.SendMessage(new EnemyOutSightEvent(this));
            _lootDropper.DropLoot();
        }

        private bool IsWithinIdleRange(Transition<EnemyState> transition) =>
            transform.position.x <= _xTolerance && transform.position.y <= _yTolerance;

        private bool IsNotWithinIdleRange(Transition<EnemyState> transition) =>
            !IsWithinIdleRange(transition);

        private bool IsAttackOver(State<EnemyState, EnemyStateEvent> state) => !_isAttack;
    }
}
