using MVCSample.Infrastructure.DI;
using MVCSample.Infrastructure.EventSystem;
using MVCSample.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    public abstract class ModuleMonoBehaviour : MonoBehaviour, IModule, IDependencyCollectionElement
    {
        [SerializeField] private BaseMonoBehaviourView _view;

        [SerializeField] private SerializableType _modelType;
        [SerializeField] private SerializableType _controllerType;

        private Context _currentContext;

        private IResolver _resolver;

        public HashSet<Type> GetNecessaryDependencesInCurrentContext()
        {


            return null;
        }

        public void Initialize(Context context)
        {
            _currentContext = context;

            PreInitialize();

            RegisterEvents(context.EventContainer);

            InitializeChildren(context);

            ResolveBindings();

            InstallBindings();

            Initialize();
        }

        private void InitializeChildren(Context context)
        {
            List<IModule> children = GetChildren();

            DependencyCollectionInitializator<IModule> initializator = new(children, 
                m => m.Initialize(context.CreateNext()));

            initializator.Initialize();
        }

        private List<IModule> GetChildren()
        {
            List<IModule> children = new();

            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).TryGetComponent(out IModule module))
                    children.Add(module);

            return children;
        }

        private void ResolveBindings()
        {
            _resolver = _currentContext.DiContainer.ContainerAPI.Resolve<IResolver>();

            _resolver.Resolve(this);
        }

        private void InstallBindings()
        {
            _currentContext.DiContainer.ContainerAPI.RegisterInstance(_resolver.GetNext(_currentContext));

            IntallBindings(_currentContext.DiContainer);
        }

        public virtual HashSet<Type> GetAllProvidedContracts() => new();

        protected virtual void RegisterEvents(EventContainer eventContainer) { }
        protected virtual void IntallBindings(IDIContainer diContainer) { }
        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }
    }
}
