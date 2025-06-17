using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public interface IDIResolverAPI
    {
        T Resolve<T>();

        bool HasBinding(Type type);
        HashSet<Type> GetAllContracts();
    }
}
