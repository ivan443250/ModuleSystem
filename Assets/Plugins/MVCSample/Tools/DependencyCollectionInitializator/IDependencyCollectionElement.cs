using MVCSample.Infrastructure;
using System;
using System.Collections.Generic;

namespace MVCSample.Tools
{
    public interface IDependencyCollectionElement
    {
        HashSet<Type> GetNecessaryDependencesInCurrentContext(Context parentContext, IEnumerable<IModule> parentChildrens);

        HashSet<Type> GetAllProvidedContracts();
    }
}
