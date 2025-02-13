using MVCSample.Infrastructure.DI;

namespace MVCSample.Infrastructure.Contexts
{
    public class Context
    {
        private readonly Context _parentContext;

        private IDIContainer _diContainer;

        public Context(IDIContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public Context(Context parentContext)
        {
            _parentContext = parentContext;
            _diContainer = GlobalContext
                .TempGetContext()
                .ResolveDependency<IDIContainer>();
        }

        public T ResolveDependency<T>()
        {
            if (_diContainer.ContainerAPI.GetAllContracts().Contains(typeof(T)))
                return _diContainer.ContainerAPI.Resolve<T>();

            return _parentContext.ResolveDependency<T>();
        }
    }
}
