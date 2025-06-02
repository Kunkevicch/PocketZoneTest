using System;
using Zenject;

namespace PocketZoneTest
{
    public class HealthPresenter : IInitializable, IDisposable
    {
        private readonly HealthView _view;
        private readonly IPlayer _player;

        public HealthPresenter(HealthView view, IPlayer player)
        {
            _view = view;
            _player = player;
        }

        public void Initialize()
        {
            _player.PlayerHealthChanged += PlayerHealthChanged;
        }

        public void Dispose()
        {
            _player.PlayerHealthChanged -= PlayerHealthChanged;
        }

        private void PlayerHealthChanged(float healthPercent)
        {
            _view.UpdateHealth(healthPercent);
        }
    }
}
