using System;

namespace PocketZoneTest
{
    public interface IPlayer
    {
        public event Action PlayerDead;
        public event Action<float> PlayerHealthChanged;
    }
}
