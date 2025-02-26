using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure
{
    public abstract class Module : IModule
    {
        public abstract HashSet<Type> GetNecessaryDependencyTypes();

        public void Initialize(Context context)
        {
            throw new NotImplementedException();
        }

        public bool TryGetContract<T>(out T contract)
        {
            throw new NotImplementedException();
        }
    }
}
