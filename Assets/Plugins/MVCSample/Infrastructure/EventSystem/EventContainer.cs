using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.EventSystem
{
    public class EventContainer
    {
        Dictionary<Type, Action<IEventData>> _eventTypePairs;

        public EventContainer()
        {
            _eventTypePairs = new();
        }

        public void RegisterEvent<T>()
        {
            RegisterEvent(typeof(T));
        }

        public void RegisterEvent(Type type)
        {
            if (_eventTypePairs.ContainsKey(type))
                throw new Exception($"Event of type {type} already exist in current context");

            _eventTypePairs.Add(type, _ => { });
        }

        public IDisposable Subscribe<T>(Action<T> genericAction) where T : IEventData
        {
            Type type = typeof(T);

            if (_eventTypePairs.ContainsKey(type) == false)
                throw new Exception($"Event of type {type} does not exist in current context");

            Action<IEventData> action = e => genericAction.Invoke((T)e);

            _eventTypePairs[type] += action;
            return new DisposableMethod(() => _eventTypePairs[type] -= action);
        }

        public void InvokeEvent<T>(IEventData e) where T : IEventData
        {
            InvokeEvent(typeof(T), e);
        }

        public void InvokeEvent(Type type, IEventData e)
        {
            if (_eventTypePairs.ContainsKey(type) == false)
                throw new Exception($"Event of type {type} does not exist in current context");

            _eventTypePairs[type].Invoke(e);
        }

        public bool HasEvent<T>() where T : IEventData
        {
            return HasEvent(typeof(T));
        }

        public bool HasEvent(Type type)
        {
            return _eventTypePairs.ContainsKey(type);
        }

        private struct DisposableMethod : IDisposable
        {
            private Action _disposableMethod;

            public DisposableMethod(Action disposableMethod)
            {
                _disposableMethod = disposableMethod;
            }

            public void Dispose()
            {
                _disposableMethod.Invoke();
            }
        }
    }
}