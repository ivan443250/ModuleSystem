using System;
using System.Collections.Generic;

namespace MVCSample.Tools
{
    public interface IDependencyCollectionElement
    {
        HashSet<Type> GetNecessaryDependencesInCurrentContext();

        HashSet<Type> GetAllProvidedContracts();
    }
}
