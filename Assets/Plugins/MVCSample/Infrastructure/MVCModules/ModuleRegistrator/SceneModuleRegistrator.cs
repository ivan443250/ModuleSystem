using MVCSample.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCSample.Infrastructure
{
    public class SceneModuleRegistrator : IModuleRegistrator
    {
        private IRegistrator[] _registrators;

        private Dictionary<IModule, DisposableObject> _moduleDisposeCallbacks;

        public SceneModuleRegistrator(IRegistrator[] registrators)
        {
            _registrators = registrators;

            _moduleDisposeCallbacks = new();
        }

        public async ValueTask DisposeAsync()
        {
            foreach (IRegistrator registrator in _registrators)
                await registrator.DisposeAsync();
        }

        public void Register(IModule module)
        {
            if (_moduleDisposeCallbacks.ContainsKey(module))
                throw new System.Exception();

            DisposableObject disposeCallback = new(() => { });

            foreach (IRegistrator registration in _registrators)
                disposeCallback += registration.Register(module);

            _moduleDisposeCallbacks.Add(module, disposeCallback);
        }

        public void Unregister(IModule module)
        {
            if (_moduleDisposeCallbacks.ContainsKey(module) == false)
                return;

            _moduleDisposeCallbacks[module].Dispose();

            _moduleDisposeCallbacks.Remove(module);
        }
    }
}
