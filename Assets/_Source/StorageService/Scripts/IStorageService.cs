using System;

namespace PocketZoneTest
{
    public interface IStorageService
    {
        public void Save(string key, object data, Action<bool> callback = null);
        public void Load<T>(string key, Action<T> callback);
        public void Reset(string key);
    }
}
