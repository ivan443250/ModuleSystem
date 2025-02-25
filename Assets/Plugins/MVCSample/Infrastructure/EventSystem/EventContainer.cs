using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.EventSystem
{
    public class EventContainer
    {
        Dictionary<Type, Action<IEvent>> _eventTypePairs;

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

        public IDisposable Subscribe<T>(Action<T> genericAction) where T : IEvent
        {
            Type type = typeof(T);

            if (_eventTypePairs.ContainsKey(type) == false)
                throw new Exception($"Event of type {type} does not exist in current context");

            Action<IEvent> action = e => genericAction.Invoke((T)e);

            _eventTypePairs[type] += action;
            return new DisposableMethod(() => _eventTypePairs[type] -= action);
        }

        public void InvokeEvent<T>(IEvent e) where T : IEvent
        {
            InvokeEvent(typeof(T), e);
        }

        public void InvokeEvent(Type type, IEvent e)
        {
            if (_eventTypePairs.ContainsKey(type) == false)
                throw new Exception($"Event of type {type} does not exist in current context");

            _eventTypePairs[type].Invoke(e);
        }

        public bool HasEvent<T>() where T : IEvent
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