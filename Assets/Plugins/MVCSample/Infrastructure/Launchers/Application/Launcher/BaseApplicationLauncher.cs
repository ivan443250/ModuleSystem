using MVCSample.Infrastructure;
using MVCSample.Infrastructure.DI;

namespace MVCSample.Infrastructure
{
    public abstract class BaseApplicationLauncher
    {
        private Context _globalContext;

        public void Construct()
        {
            PreInitialize();

            _globalContext = new Context(CreateContainer());

            InstallBindings(_globalContext.DiContainer);
            ResolveDependences(_globalContext.DiContainer);

            Initialize();
        }

        protected abstract IDIContainer CreateContainer();

        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }

        protected virtual void InstallBindings(IDIContainer container) { }
        protected virtual void ResolveDependences(IDIContainer container) { }
    }
}
