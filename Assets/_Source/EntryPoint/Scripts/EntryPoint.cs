using UnityEngine;
using Zenject;

namespace PocketZoneTest
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        private Player _player;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
        }
        private void Awake()
        {
            _player.transform.position = _spawnPoint.position;
        }
    }
}
