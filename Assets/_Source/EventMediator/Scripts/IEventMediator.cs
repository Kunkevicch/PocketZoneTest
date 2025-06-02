using System;

namespace PocketZoneTest
{
    public interface IEventMediator
    {
        void AddListener<T>(Action<T> callback) where T : struct;
        void RemoveListener<T>(Action<T> callback) where T : struct;
        void SendMessage<T>(T eventData) where T : struct;
    }
}
