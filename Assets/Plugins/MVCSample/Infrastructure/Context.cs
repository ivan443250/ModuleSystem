using MVCSample.Infrastructure.DI;
using MVCSample.Infrastructure.EventSystem;
using System;

namespace MVCSample.Infrastructure
{
    public class Context
    {
        public readonly IDIContainer DiContainer;
        public readonly EventContainer @EventContainer;

        private readonly Context _parentContext;

        public Context()
        {
            @EventContainer = new();
        }

        //Global Context
        public Context(IDIContainer dIContainer) : this() 
        {
            DiContainer = dIContainer;

            Type containerType = DiContainer.GetType();

            DiContainer.ContainerAPI.RegisterFromMethod(() => Activator.CreateInstance(containerType) as IDIContainer);
        }

        //Other Contexts
        public Context(Context parentContext) : this()
        {
            _parentContext = parentContext;

            DiContainer = _parentContext.ResolveDeep<IDIContainer>();
        }

        public Context CreateNext()
        {
            return new Context(this);
        }

        public T ResolveDeep<T>()
        {
            if (DiContainer.ContainerAPI.HasBinding(typeof(T)))
                return DiContainer.ContainerAPI.Resolve<T>();

            if (_parentContext == null)
                throw new System.Exception();

            return _parentContext.ResolveDeep<T>();
        }

        public IDisposable SubscribeDeep<T>(Action<T> action) where T : IEvent
        {
            if (@EventContainer.HasEvent<T>())
                return @EventContainer.Subscribe(action);

            if (_parentContext == null)
                throw new System.Exception();

            return _parentContext.SubscribeDeep(action);
        }
    }
}
