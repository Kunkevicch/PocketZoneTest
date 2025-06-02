using UnityEngine;
using UnityEngine.UI;

namespace PocketZoneTest
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image _hp;

        public void UpdateHealth(float health)
        {
            _hp.fillAmount = health;
        }
    }
}
