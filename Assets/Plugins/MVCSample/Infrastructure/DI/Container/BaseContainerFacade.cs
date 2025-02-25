using MVCSample.Tools;
using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public abstract class BaseContainerFacade<ContainerT> : IDIContainer, IBaseDIContainerAPI
    {
        public IBaseDIContainerAPI ContainerAPI => this;

        protected readonly ContainerT Container;

        public BaseContainerFacade()
        {
            Container = CreateContainer();
        }

        public void AddInstallers(IEnumerable<IInstaller> installers)
        {
            foreach (IInstaller installer in installers)
                InstallerConditionHandler.HandleInstallerConditions(installer, Container, this);
        }

        public abstract ContainerT CreateContainer();

        #region ContainerAPI
        public abstract void Register<TInterface, TImplementation>(bool asSingle = true) where TImplementation : TInterface;
        public abstract void RegisterInstance<TInterface>(TInterface instance);
        public abstract void RegisterFromMethod<TInterface>(Func<TInterface> method);

        public abstract T Resolve<T>();
        public abstract bool HasBinding(Type type);

        public abstract HashSet<Type> GetAllContracts();
        #endregion
    }
}
