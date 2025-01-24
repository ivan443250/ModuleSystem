using System;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    public interface IResolver
    {
        Type GetDependentType();
        object Resolve();
    }
}
