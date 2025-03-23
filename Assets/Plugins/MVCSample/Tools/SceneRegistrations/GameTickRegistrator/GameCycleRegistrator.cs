using MVCSample.Infrastructure;
using System.Collections.Generic;

namespace MVCSample.Tools
{
    public class GameCycleRegistrator : IRegistrator
    {
        private List<ITickable> _tickables;
        private List<IFixedTickable> _fixedTickables;

        private IGameLoop _gameLoop;

        public GameCycleRegistrator(IGameLoop gameLoop)
        {
            _gameLoop = gameLoop;

            _gameLoop.UpdateCallback += Update;
            _gameLoop.FixedUpdateCallback += FixedUpdate;

            _tickables = new();
            _fixedTickables = new();
        }

        public DisposableObject Register(IModule objectToRegister)
        {
            return RegisterTickable(objectToRegister) + RegisterFixedTickable(objectToRegister);
        }

        private DisposableObject RegisterTickable(IModule tickableObject)
        {
            if (tickableObject is not ITickable tickable)
                return new DisposableObject(() => { });

            _tickables.Add(tickable);

            return new DisposableObject(() => _tickables.Remove(tickable));
        }

        private DisposableObject RegisterFixedTickable(IModule fixedTickableObject)
        {
            if (fixedTickableObject is not IFixedTickable fixedTickable)
                return new DisposableObject(() => { });

            _fixedTickables.Add(fixedTickable);

            return new DisposableObject(() => _fixedTickables.Remove(fixedTickable));
        }

        private void Update(float tickDelta)
        {
            if (_tickables.Count == 0)
                return;

            foreach (ITickable tickable in _tickables)
                tickable.Tick(tickDelta);
        }

        private void FixedUpdate(float fixedTickDelta)
        {
            if (_fixedTickables.Count == 0)
                return;

            foreach (IFixedTickable tickable in _fixedTickables)
                tickable.FixedTick(fixedTickDelta);
        }

        public void Dispose()
        {
            _gameLoop.UpdateCallback -= Update;
            _gameLoop.FixedUpdateCallback -= FixedUpdate;
        }
    }
}
