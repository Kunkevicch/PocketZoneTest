using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PocketZoneTest
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _enemyPrefabs;
        [SerializeField] private int _enemyCount;
        [SerializeField] private Transform _enemySpawnStart;
        [SerializeField] private Transform _enemySpawnEnd;

        private DiContainer _container;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }

        private void Start()
        {
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                Vector2 randomPosition = GetRandomPositionInArea();

                _container.InstantiatePrefabForComponent<Enemy>(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)], randomPosition, Quaternion.identity, null);
            }
        }

        private Vector2 GetRandomPositionInArea()
        {
            float randomX = Random.Range(_enemySpawnStart.position.x, _enemySpawnEnd.position.x);
            float randomY = Random.Range(_enemySpawnStart.position.y, _enemySpawnEnd.position.y);

            return new Vector2(randomX, randomY);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 center = (_enemySpawnStart.position + _enemySpawnEnd.position) / 2;
            Vector3 size = new(_enemySpawnEnd.position.x - _enemySpawnStart.position.x, _enemySpawnEnd.position.y - _enemySpawnStart.position.y, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
