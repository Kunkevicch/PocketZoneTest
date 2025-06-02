using System;
using UnityEngine;

namespace PocketZoneTest
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _currentHealth;

        public event Action<int> HealthChanged;
        public event Action Dead;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public int MaxHealth => _maxHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (_currentHealth == value)
                    return;

                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);

                HealthChanged?.Invoke(_currentHealth);

                if (_currentHealth == 0)
                {
                    Dead?.Invoke();
                }
            }
        }
        public float CurrentHealthPercent => (float)_currentHealth / _maxHealth;

        public void DealDamage(int damage)
        {
            if (damage < 0)
                return;
            CurrentHealth -= damage;
        }
    }
}
