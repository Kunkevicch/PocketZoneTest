using System;
using System.Collections.Generic;

namespace PocketZoneTest
{
    public class EventMediator :IEventMediator
    {
        private readonly Dictionary<Type, Delegate> _listeners = new();

        public void AddListener<T>(Action<T> callback) where T : struct
        {
            Type eventType = typeof(T);
            if (_listeners.TryGetValue(eventType, out var existingDelegate))
            {
                _listeners[eventType] = Delegate.Combine(existingDelegate, callback);
            }
            else
            {
                _listeners[eventType] = callback;
            }
        }

        public void RemoveListener<T>(Action<T> callback) where T : struct
        {
            Type eventType = typeof(T);
            if (_listeners.TryGetValue(eventType, out var existingDelegate))
            {
                var newDelegate = Delegate.Remove(existingDelegate, callback);
                if (newDelegate == null)
                {
                    _listeners.Remove(eventType);
                }
                else
                {
                    _listeners[eventType] = newDelegate;
                }
            }
        }

        public void SendMessage<T>(T eventData) where T : struct
        {
            Type eventType = typeof(T);
            if (_listeners.TryGetValue(eventType, out var delegateObj))
            {
                (delegateObj as Action<T>)?.Invoke(eventData);
            }
        }
    }
}
