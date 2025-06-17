using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public interface IResolvingChecker 
    {
        bool CheckResolving(Context context, IEnumerable<Type> dependences, out HashSet<Type> unresolvableTypes);
        IEnumerable<Type> GetAllDependences(Context context, Type type);
    }
}
