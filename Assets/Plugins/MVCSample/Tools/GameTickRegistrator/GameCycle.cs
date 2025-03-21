using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVCSample.Tools
{
    public class GameCycle : IGameCycle
    {
        private List<ITickable> _tickables;
        private List<IFixedTickable> _fixedTickables;

        public GameCycle()
        {
            _tickables = new();
            _fixedTickables = new();
        }

        public IDisposable RegisterTickable(GameObject tickableObject)
        {
            if (tickableObject.TryGetComponent(out ITickable tickable))
                _tickables.Add(tickable);

            return new InternalDisposable(() => _tickables.Remove(tickable));
        }

        public IDisposable RegisterFixedTickable(GameObject fixedTickableObject)
        {
            if (fixedTickableObject.TryGetComponent(out IFixedTickable fixedTickable))
                _fixedTickables.Add(fixedTickable);

            return new InternalDisposable(() => _fixedTickables.Remove(fixedTickable));
        }

        public void Update(float tickDelta)
        {
            if (_tickables.Count == 0)
                return;

            foreach (ITickable tickable in _tickables)
                tickable.Tick(tickDelta);
        }

        public void FixedUpdate(float fixedTickDelta)
        {
            if (_fixedTickables.Count == 0)
                return;

            foreach (IFixedTickable tickable in _fixedTickables)
                tickable.FixedTick(fixedTickDelta);
        }

        public struct InternalDisposable : IDisposable
        {
            private Action _disposeCallback;

            public InternalDisposable(Action disposeCallback)
            {
                _disposeCallback = disposeCallback;
            }

            public void Dispose()
            {
                _disposeCallback.Invoke();
            }
        }
    }
}
