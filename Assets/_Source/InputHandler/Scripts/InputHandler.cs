using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PocketZoneTest
{
    public class InputHandler : MonoBehaviour, IInput
    {
        [SerializeField] private Button _fireButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Joystick _joystickMove;

        public event Action<Vector2> MoveToDirection;
        public event Action Fire;
        public event Action SwitchInventoryState;

        private IEventMediator _eventMediator;

        [Inject]
        public void Construct(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        private void OnEnable()
        {
            StartCoroutine(MoveToDirectionRoutine());
            _fireButton.onClick.AddListener(() => Fire?.Invoke());
            _inventoryButton.onClick.AddListener(() => SwitchInventoryState?.Invoke());

            _eventMediator.AddListener<AllEnemiesOutSightEvent>(OnAllenemiesOutSight);
            _eventMediator.AddListener<EnemyInSightEvent>(OnEnemyInSight);
        }


        private void OnDisable()
        {
            _fireButton.onClick.RemoveListener(() => Fire?.Invoke());
            _inventoryButton.onClick.RemoveListener(() => SwitchInventoryState?.Invoke());

            _eventMediator.RemoveListener<AllEnemiesOutSightEvent>(OnAllenemiesOutSight);
            _eventMediator.RemoveListener<EnemyInSightEvent>(OnEnemyInSight);
        }

        private IEnumerator MoveToDirectionRoutine()
        {
            while (true)
            {
                yield return new WaitUntil(() => _joystickMove.Direction != Vector2.zero);

                while (_joystickMove.Direction != Vector2.zero)
                {
                    MoveToDirection?.Invoke(_joystickMove.Direction.normalized);
                    yield return null;
                }

                MoveToDirection?.Invoke(Vector2.zero);
            }
        }

        private void OnAllenemiesOutSight(AllEnemiesOutSightEvent @event)
        => _fireButton.interactable = false;

        private void OnEnemyInSight(EnemyInSightEvent @event)
        => _fireButton.interactable = true;
    }
}