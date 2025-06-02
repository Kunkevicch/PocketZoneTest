using UnityEngine;

namespace PocketZoneTest
{
    public static class GameObjectExtension
    {
        public static bool IsInLayer(this GameObject go, LayerMask layerMask)
        {
            return layerMask == (layerMask | 1 << go.layer);
        }
    }
}
