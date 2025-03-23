using System;
using UnityEngine;

namespace MVCSample.Tools
{
    public interface IGameLoop
    {
        public event Action<float> UpdateCallback;
        public event Action<float> FixedUpdateCallback;
    }
}