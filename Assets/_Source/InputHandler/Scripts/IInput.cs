using System;
using UnityEngine;

namespace PocketZoneTest
{
    public interface IInput
    {
        public event Action<Vector2> MoveToDirection;
        public event Action Fire;
        public event Action SwitchInventoryState;
    }
}
