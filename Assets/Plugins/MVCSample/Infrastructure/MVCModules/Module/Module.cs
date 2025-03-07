using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure
{
    public abstract class Module : IModule
    {
        public HashSet<Type> GetAllProvidedContracts()
        {
            throw new NotImplementedException();
        }

        public abstract HashSet<Type> GetNecessaryDependencesInCurrentContext();

        public void Construct(Context context)
        {
            throw new NotImplementedException();
        }

        public bool TryGetContract<T>(out T contract)
        {
            throw new NotImplementedException();
        }

        public abstract HashSet<Type> GetNecessaryDependencesInCurrentContext(Context parentContext);
    }
}
