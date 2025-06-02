using System;
using UnityEngine;
using Zenject;

namespace PocketZoneTest
{
    public class PlayerCombat : MonoBehaviour
    {
        private IWeapon _weapon;
        private IInventoryController _inventoryController;

        [Inject]
        public void Construct(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public event Action<int> AmmoChanged;

        private void OnEnable()
        {
            _inventoryController.InventoryChanged += OnInventoryChanged;
        }

        private void OnDisable()
        {
            _inventoryController.InventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged()
        => AmmoChanged?
            .Invoke(
                Mathf.Clamp(
                    _inventoryController.GetItemCount(_weapon.GetAmmoData())
                    , 0
                    , _weapon.GetMaxAmmo()
                    )
                );

        public void SetWeapon(IWeapon weapon)
        {
            _weapon = weapon;
            AmmoChanged?.Invoke(_weapon.GetAmmo());
        }

        public void Attack()
        {
            if (_weapon.GetAmmo() == 0)
            {
                Reload();
            }
            _weapon.Attack();
            AmmoChanged?.Invoke(_weapon.GetAmmo());
        }

        private void Reload()
        {
            var ammo = _inventoryController
                .TryConsumeItemFromMultipleStacks(_weapon.GetAmmoData(), _weapon.GetMaxAmmo());

            if (ammo > 0)
            {
                _weapon.Reload(ammo);
            }
        }
    }
}
