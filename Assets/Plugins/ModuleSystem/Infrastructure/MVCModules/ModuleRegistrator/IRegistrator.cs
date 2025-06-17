using MVCSample.Tools;
using System;

namespace MVCSample.Infrastructure
{
    public interface IRegistrator : IAsyncDisposable
    {
        DisposableObject Register(IModule objectToRegister);
    }
}