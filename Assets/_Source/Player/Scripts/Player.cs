using System;
using UnityEngine;

namespace PocketZoneTest
{
    public class Player : MonoBehaviour, IPlayer
    {
        private Health _health;
        private bool _isDead;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        public event Action PlayerDead;
        public event Action<float> PlayerHealthChanged;

        private void OnEnable()
        {
            _health.HealthChanged += OnHealthChanged;
            _health.Dead += OnDead;
        }

        private void OnDisable()
        {
            _health.HealthChanged -= OnHealthChanged;
            _health.Dead -= OnDead;
        }

        private void OnHealthChanged(int newHealth)
        {
            if (_isDead)
                return;

            PlayerHealthChanged?.Invoke(_health.CurrentHealthPercent);
        }

        private void OnDead()
        {
            if (_isDead)
                return;
            _isDead = true;
            PlayerDead?.Invoke();
        }
    }
}
