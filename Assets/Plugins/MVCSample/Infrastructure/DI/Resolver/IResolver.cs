using System;

namespace MVCSample.Infrastructure.DI
{
    public interface IResolver
    {
        void Resolve(Context context, object resolvable);
    }
}
