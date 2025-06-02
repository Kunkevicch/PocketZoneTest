using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PocketZoneTest
{
    [Serializable]
    public class SaveDataInventory
    {
        [JsonProperty(PropertyName = "Items")]
        public Dictionary<int, InventorySaveDataDetails> InventoryItems;

        public SaveDataInventory()
        {
            InventoryItems = new();
        }
    }

    public struct InventorySaveDataDetails
    {
        public int quantity;
        public string itemID;
        public bool IsEmpty;
    }
}
