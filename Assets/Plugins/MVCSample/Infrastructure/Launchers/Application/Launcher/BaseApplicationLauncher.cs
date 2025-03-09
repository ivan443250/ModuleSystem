using MVCSample.Infrastructure.DI;

namespace MVCSample.Infrastructure
{
    public abstract class BaseApplicationLauncher
    {
        protected Context GlobalContext { get; private set; }

        public void Construct()
        {
            PreInitialize();

            GlobalContext = new Context(CreateContainer());

            InstallBindings(GlobalContext.DiContainer);

            Initialize();
        }

        protected abstract IDIContainer CreateContainer();

        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }

        protected virtual void InstallBindings(IDIContainer container) { }
    }
}
