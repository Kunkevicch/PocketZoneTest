using UnityEngine;
using Zenject;

namespace PocketZoneTest
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private int _damage;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private LayerMask _contactLayer;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private ItemData _ammoData;
        [SerializeField] private int _maxAmmoInClip;

        private int _currentAmmo;

        private ObjectPool _objectPool;

        [Inject]
        public void Construct(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        public void Attack()
        {
            if (CanAttack())
            {
                _currentAmmo--;
                var projectile = (ILaunchable)_objectPool.ReuseComponent(_projectilePrefab, _firePoint.position, transform.rotation, true);
                projectile.Launch(_firePoint.right, _damage);
            }
        }

        public int GetAmmo() => _currentAmmo;

        private bool CanAttack() => _currentAmmo > 0;

        public void Reload(int ammo) => _currentAmmo = ammo;

        public int GetMaxAmmo() => _maxAmmoInClip;

        public ItemData GetAmmoData() => _ammoData;

    }
}
