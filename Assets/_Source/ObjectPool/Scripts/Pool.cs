using System;
using UnityEngine;

namespace PocketZoneTest
{
    [Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;
        public bool isInjected;
    }
}