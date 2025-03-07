using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public interface IResolvingChecker 
    {
        bool CheckResolving(Context context, Type type, out HashSet<Type> unresolvableTypes);
    }
}
