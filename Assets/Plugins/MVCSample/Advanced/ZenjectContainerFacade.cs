using MVCSample.Infrastructure.DI;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override bool HasBinding(Type type)
        {
            return Container.HasBinding(type);
        }

        public override IEnumerable<Type> GetAllContracts()
        {
            return Container.AllContracts.Select(binding => binding.Type);
        }
    }
}
