using MVCSample.Tools;
using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public abstract class BaseContainerFacade<ContainerT> : IDIContainer, IBaseDIContainerAPI
    {
        protected readonly ContainerT Container;

        public BaseContainerFacade()
        {
            Container = CreateContainer();
        }

        public void AddInstallers(IEnumerable<IInstaller> installers)
        {
            InstallerConditionHandler<ContainerT> installerHandler = new(Container, this);

            foreach (IInstaller installer in installers)
                installerHandler.HandleInstallerConditions(installer);
        }

        public IBaseDIContainerAPI GetContainerAPI()
        {
            return this;
        }

        public abstract ContainerT CreateContainer();

        #region ContainerAPI
        public abstract void Register<TInterface, TImplementation>(bool asSingle = true) where TImplementation : TInterface;
        public abstract void RegisterInstance<TInterface>(TInterface instance);

        public abstract T Resolve<T>();
        public abstract bool HasBinding(Type type);

        public abstract IEnumerable<Type> GetAllContracts();
        #endregion
    }
}
