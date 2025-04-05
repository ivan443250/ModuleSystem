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
            if (_children.Count == 0)
                return;

            if (_children.Count == 1)
            {
                InitializeModule(_children.First(), initializationContext);
                return;
            }

            DependencyCollectionInitializator<IModule> initializator = new(_children,
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

        public SceneModule CreateChild(SceneModule childPrefab, Context parentContext)
        {
            SceneModule instance = UnityEngine.Object.Instantiate(childPrefab, _currentModuleTransform);

            _children.Add(instance);

            InitializeModule(instance, parentContext);

            return instance;
        }

        public void DestroyChild(SceneModule child)
        {
            if (_children.Contains(child) == false)
                throw new Exception("Attempt to delete an object that does not exist in the current module");

            _children.Remove(child);

            ((IModule)child).Dispose();

            UnityEngine.Object.Destroy(child.gameObject);
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