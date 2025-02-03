using MVCSample.Infrastructure.DI;
using Zenject;

namespace MVCSample.Advanced
{
    public class ZenjectContainerFacade : BaseContainerFacade<DiContainer>
    {
        public override DiContainer CreateContainer()
        {
            return new DiContainer();
        }

        public override void Register<TInterface, TImplementation>(bool asSingle = true)
        {
            if (asSingle) 
                Container.Bind<TInterface>().To<TImplementation>().AsSingle();
            else
                Container.Bind<TInterface>().To<TImplementation>().AsTransient();
        }

        public override void RegisterInstance<TInterface>(TInterface instance)
        {
            Container.Bind<TInterface>().FromInstance(instance).AsSingle();
        }

        public override T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public override bool TryResolve<T>(out T instance) where T : class
        {
            instance = Container.TryResolve<T>();
            return instance != null;
        }

        public override bool HasBinding<T>()
        {
            return Container.HasBinding<T>();
        }
    }
}
