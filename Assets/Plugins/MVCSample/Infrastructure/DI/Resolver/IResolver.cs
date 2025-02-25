using System;

namespace MVCSample.Infrastructure.DI
{
    public interface IResolver
    {
        void Resolve(object resolvable, Type type);
    }
}
