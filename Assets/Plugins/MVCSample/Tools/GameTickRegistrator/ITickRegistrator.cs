using System;
using UnityEngine;

namespace MVCSample.Tools
{
    public interface ITickRegistrator
    {
        IDisposable RegisterTickable(GameObject tickable);
        IDisposable RegisterFixedTickable(GameObject fixedTickable);
    }
}
