using System;
using UnityEngine;

namespace MVCSample.Tools
{
    public class GameLoop : IGameLoop
    {
        public event Action<float> UpdateCallback
        {
            add { _component.UpdateCallback += value; Debug.Log("UpdateCallback add"); }
            remove { _component.UpdateCallback -= value; Debug.Log("UpdateCallback remove"); }
        }

        public event Action<float> FixedUpdateCallback
        {
            add { _component.FixedUpdateCallback += value; Debug.Log("FixedUpdateCallback add"); }
            remove { _component.FixedUpdateCallback -= value; Debug.Log("FixedUpdateCallback remove"); }
        }

        private LoopComponent _component;

        public GameLoop()
        {
            _component = new GameObject("GameLoop").AddComponent<LoopComponent>();
            UnityEngine.Object.DontDestroyOnLoad(_component);
        }
    }
}
