using System;
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

        public event Action Fire;
        public event Action SwitchInventoryState;

        private IEventMediator _eventMediator;


        [Inject]
        public void Construct(IEventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }
        public Vector2 InputDirection => _joystickMove.Direction;

        private void OnEnable()
        {
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

        private void OnAllenemiesOutSight(AllEnemiesOutSightEvent @event)
        => _fireButton.interactable = false;

        private void OnEnemyInSight(EnemyInSightEvent @event)
        => _fireButton.interactable = true;
    }
}