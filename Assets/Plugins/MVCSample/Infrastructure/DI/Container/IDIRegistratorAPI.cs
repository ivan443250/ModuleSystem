using System;
using UnityEngine;

namespace MVCSample.Infrastructure.DI
{
    public interface IDIRegistratorAPI
    {
        void Register<TInterface, TImplementation>(bool asSingle = true) where TImplementation : TInterface;
        void RegisterInstance<TInterface>(TInterface instance);
        void RegisterFromMethod<TInterface>(Func<TInterface> method);
    }
}
