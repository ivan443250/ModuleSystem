using System;

namespace MVCSample.Infrastructure
{
    public interface IModuleRegistrator : IAsyncDisposable
    {
        void Register(IModule module);
        void Unregister(IModule module);
    }
}