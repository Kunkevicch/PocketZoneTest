using UnityEngine;
using UnityEngine.UI;

namespace PocketZoneTest
{
    public class UIInfo : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Image _healthBarFiller;
        private void OnEnable()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int newHealth)
        {
            _healthBarFiller.fillAmount = _health.CurrentHealthPercent;
        }
    }
}
