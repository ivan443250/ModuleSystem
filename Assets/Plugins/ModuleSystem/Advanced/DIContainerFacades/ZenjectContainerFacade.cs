using MVCSample.Infrastructure.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MVCSample.Advanced
{
    public class ZenjectContainerFacade : BaseContainerFacade<DiContainer>
    {
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

        public override void RegisterFromMethod<TInterface>(Func<TInterface> method)
        {
            Container.Bind<TInterface>().FromMethod(method).AsTransient();
        }

        public override T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public override bool HasBinding(Type type)
        {
            return Container.HasBinding(type);
        }

        public override HashSet<Type> GetAllContracts()
        {
            return new (Container.AllContracts.Select(binding => binding.Type));
        }

        protected override DiContainer CreateContainer()
        {
            return new DiContainer();
        }
    }
}
