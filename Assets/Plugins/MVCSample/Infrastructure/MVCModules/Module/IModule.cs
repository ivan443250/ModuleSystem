using MVCSample.Tools;
using System;

namespace MVCSample.Infrastructure
{
    public interface IModule : IDependencyCollectionElement, IDisposable
    {
        void Construct(Context context);
    }
}