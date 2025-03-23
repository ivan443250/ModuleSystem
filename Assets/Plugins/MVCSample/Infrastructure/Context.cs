using MVCSample.Infrastructure.DI;
using MVCSample.Infrastructure.EventSystem;
using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure
{
    public class Context
    {
        public static IDIResolverAPI Global { get; private set; }

        public readonly IDIContainer DiContainer;
        public readonly EventContainer @EventContainer;

        private readonly Context _parentContext;

        private HashSet<Type> _bindWaitings;

        private Context()
        {
            @EventContainer = new();

            _bindWaitings = new();
        }

        //Global Context
        public Context(IDIContainer dIContainer) : this()
        {
            if (Global != null)
                throw new Exception("Can not use this constructor when global context already exist");

            DiContainer = dIContainer;

            Global = DiContainer.ContainerAPI;
        }

        //Other Contexts
        private Context(Context parentContext) : this()
        {
            _parentContext = parentContext;

            DiContainer = Global.Resolve<IDIContainer>();
        }

        public Context CreateNext()
        {
            return new Context(this);
        }

        public void AddBindWaiting(Type type)
        {
            if (_bindWaitings.Contains(type))
                throw new Exception(/*todo*/);

            _bindWaitings.Add(type);
        }

        public bool TryRegisterBindInWaitingDeep(Type type, Action<IDIContainer> bindCallback)
        {
            if (_bindWaitings.Contains(type))
            {
                bindCallback.Invoke(DiContainer);
                _bindWaitings.Remove(type);
                return true;
            }

            if (_parentContext == null)
                return false;

            return _parentContext.TryRegisterBindInWaitingDeep(type, bindCallback);
        }

        public bool HasBindingDeep(Type type)
        {
            if (DiContainer.ContainerAPI.HasBinding(type))
                return true;

            return _parentContext == null ? false : _parentContext.HasBindingDeep(type);
        }

        public T ResolveDeep<T>()
        {
            if (DiContainer.ContainerAPI.HasBinding(typeof(T)))
                return DiContainer.ContainerAPI.Resolve<T>();

            if (_parentContext == null)
                throw new Exception($"Type {typeof(T)} was not binded yet");

            return _parentContext.ResolveDeep<T>();
        }

        public IDisposable SubscribeDeep<T>(Action<T> action) where T : IEventData
        {
            if (@EventContainer.HasEvent<T>())
                return @EventContainer.Subscribe(action);

            if (_parentContext == null)
                throw new Exception(/*todo*/);

            return _parentContext.SubscribeDeep(action);
        }
    }
}
