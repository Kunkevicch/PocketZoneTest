using System;
using UnityEngine;

namespace PocketZoneTest
{
    public interface IInput
    {
        public event Action Fire;
        public event Action SwitchInventoryState;

        public Vector2 InputDirection { get; }
    }
}
