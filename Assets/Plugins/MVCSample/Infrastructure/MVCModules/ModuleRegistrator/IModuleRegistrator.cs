using System;

namespace MVCSample.Infrastructure
{
    public interface IModuleRegistrator : IDisposable
    {
        void Register(IModule module);
        void Unregister(IModule module);
    }
}