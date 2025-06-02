using TMPro;
using UnityEngine;

namespace PocketZoneTest
{
    public class TextView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _text;

        public void SetText(string newText) => _text.text = newText;
    }
}
