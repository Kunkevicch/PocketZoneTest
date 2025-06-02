using System;
using Zenject;

namespace PocketZoneTest
{
    public class AmmoPresenter : IInitializable, IDisposable
    {
        private readonly TextView _ammoView;
        private readonly PlayerCombat _playerCombat;

        public AmmoPresenter(TextView ammoView, Player player)
        {
            _ammoView = ammoView;
            _playerCombat = player.GetComponent<PlayerCombat>();
        }

        public void Initialize() => _playerCombat.AmmoChanged += OnAmmoChanged;
        public void Dispose() => _playerCombat.AmmoChanged -= OnAmmoChanged;

        private void OnAmmoChanged(int newAmmo) => _ammoView.SetText(newAmmo.ToString());
    }
}
