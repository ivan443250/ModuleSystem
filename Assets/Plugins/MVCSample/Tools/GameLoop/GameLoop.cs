using System;
using UnityEngine;

namespace MVCSample.Tools
{
    public class GameLoop : IGameLoop
    {
        public event Action<float> UpdateCallback
        {
            add => _component.UpdateCallback += value;
            remove => _component.UpdateCallback -= value;
        }

        public event Action<float> FixedUpdateCallback
        {
            add => _component.FixedUpdateCallback += value;
            remove => _component.FixedUpdateCallback -= value;
        }

        private LoopComponent _component;

        public GameLoop()
        {
            _component = new GameObject("GameLoop").AddComponent<LoopComponent>();
            UnityEngine.Object.DontDestroyOnLoad(_component);
        }
    }
}
