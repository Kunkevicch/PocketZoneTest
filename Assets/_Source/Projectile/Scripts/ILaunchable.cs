using UnityEngine;

namespace PocketZoneTest
{
    public interface ILaunchable
    {
        public void Launch(Vector2 direction, int damage);
    }
}
