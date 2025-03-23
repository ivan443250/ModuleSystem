using MVCSample.Tools;
using System;

namespace MVCSample.Infrastructure
{
    public interface IRegistrator : IDisposable
    {
        DisposableObject Register(IModule objectToRegister);
    }
}