using MVCSample.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    public class ChildModulesCollection : IEnumerable<IModule>, IDisposable
    {
        public List<IModule> Children => _children;

        private List<IModule> _children;

        private Transform _currentModuleTransform;

        private Dictionary<IModule, Action<IModule, Context>> _modulesSetups;

        public ChildModulesCollection(Transform currentModuleTransform)
        {
            _currentModuleTransform = currentModuleTransform;

            _children = GetChildren();

            _modulesSetups = new();
        }

        private List<IModule> GetChildren()
        {
            if (_children == null)
            {
                _children = new();

                for (int i = 0; i < _currentModuleTransform.childCount; i++)
                    if (_currentModuleTransform.GetChild(i).TryGetComponent(out IModuleContainer container))
                        _children.AddRange(container.GetModules());
            }

            return _children;
        }

        public void SetupChildren(Predicate<IModule> selectCallback, Action<IModule, Context> initializationCallback)
        {
            foreach (IModule module in _children.Where(m => selectCallback.Invoke(m)))
            {
                if (_modulesSetups.ContainsKey(module) == false)
                    _modulesSetups.Add(module, (_, _) => { });

                _modulesSetups[module] += initializationCallback;
            }
        }

        public void InitializeChildren(Context initializationContext)
        {
            List<IModule> children = GetChildren();

            if (children.Count == 0)
                return;

            if (children.Count == 1)
            {
                children.First().Construct(initializationContext.CreateNext());
                return;
            }

            DependencyCollectionInitializator<IModule> initializator = new(children,
                m => InitializeModule(m, initializationContext),
                m => m.GetNecessaryDependencesInCurrentContext(initializationContext, GetChildren()));

            initializator.Initialize();
        }

        public void Dispose()
        {
            foreach (IModule module in _children)
                module.Dispose();
        }

        public IEnumerator<IModule> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        private void InitializeModule(IModule module, Context initializationContext)
        {
            Context newModuleContext = initializationContext.CreateNext();

            if (_modulesSetups.ContainsKey(module))
                _modulesSetups[module].Invoke(module, newModuleContext);

            module.Construct(newModuleContext);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}