using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure
{
    public interface IModule
    {
        HashSet<Type> GetNecessaryDependencyTypes();
        
        void Initialize(Context context);

        bool TryGetContract<T>(out T contract);
    }
}