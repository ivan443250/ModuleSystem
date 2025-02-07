using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public interface IBaseDIContainerAPI
    {
        void Register<TInterface, TImplementation>(bool asSingle = true) where TImplementation : TInterface;
        void RegisterInstance<TInterface>(TInterface instance);

        T Resolve<T>();

        bool HasBinding(Type type);
        IEnumerable<Type> GetAllContracts();
    }
}
