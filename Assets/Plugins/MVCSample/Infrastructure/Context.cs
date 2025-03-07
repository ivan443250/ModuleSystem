using MVCSample.Infrastructure.DI;
using MVCSample.Infrastructure.EventSystem;
using System;

namespace MVCSample.Infrastructure
{
    public class Context
    {
        public static IDIResolverAPI Global { get; private set; }

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
            if (Global != null)
                throw new Exception("Can not use this constructor when global context already exist");

            DiContainer = dIContainer;

            Type containerType = DiContainer.GetType();

            DiContainer.ContainerAPI.RegisterFromMethod(() => Activator.CreateInstance(containerType) as IDIContainer);

            Global = DiContainer.ContainerAPI;
        }

        //Other Contexts
        public Context(Context parentContext) : this()
        {
            _parentContext = parentContext;

            DiContainer = Global.Resolve<IDIContainer>();
        }

        public Context CreateNext()
        {
            return new Context(this);
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
