using MVCSample.Infrastructure.DI;
using MVCSample.Infrastructure.EventSystem;
using MVCSample.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #region Initialize

        public void Construct(Context context)
        {
            _currentContext = context;

            PreInitialize();

            RegisterEvents(context.EventContainer);

            InitializeChildren(context);

            ResolveBindings();

            InstallBindings();

            Initialize();
        }

        public virtual HashSet<Type> GetAllProvidedContracts() => new();

        public HashSet<Type> GetNecessaryDependencesInCurrentContext(Context parentContext)
        {
            IResolvingChecker resolvingChecker = parentContext.ResolveDeep<IResolvingChecker>();

            if (resolvingChecker.CheckResolving(GetType(), out HashSet<Type> unresolvableTypes))
                return new();

            IEnumerable<Type> contractsInContext = GetChildren(transform.parent)
                .SelectMany(m => m.GetAllProvidedContracts());

            IEnumerable<Type> remainingTypes = unresolvableTypes.Except(contractsInContext);

            if (remainingTypes.Count() > 0)
                throw new Exception(CreateTypeNotFoundErrorLog(remainingTypes));

            return new(unresolvableTypes.Intersect(contractsInContext, new ReferenceComparer<Type>()));
        }

        #endregion

        #region Subtypes interface

        protected virtual void RegisterEvents(EventContainer eventContainer) { }
        protected virtual void IntallBindings(IDIContainer diContainer) { }
        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }

        #endregion

        #region Inside methods

        private void InitializeChildren(Context context)
        {
            List<IModule> children = GetChildren();

            DependencyCollectionInitializator<IModule> initializator = new(children, 
                m => m.Construct(context.CreateNext()),
                m => m.GetNecessaryDependencesInCurrentContext(context));

            initializator.Initialize();
        }

        private List<IModule> GetChildren()
        {
            return GetChildren(transform);
        }

        private List<IModule> GetChildren(Transform parent)
        {
            List<IModule> children = new();

            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).TryGetComponent(out IModule module))
                    children.Add(module);

            return children;
        }

        private void ResolveBindings()
        {
            _resolver = _currentContext.ResolveDeep<IResolver>();

            _resolver.Resolve(this);
        }

        private void InstallBindings()
        {
            _currentContext.DiContainer.ContainerAPI.RegisterInstance(_resolver.GetNext(_currentContext));

            IntallBindings(_currentContext.DiContainer);
        }

        #endregion

        #region Logs

        private string CreateTypeNotFoundErrorLog(IEnumerable<Type> unfoundedTypes)
        {
            StringBuilder sb = new($"Type {GetType()} can not get some types in current context:\n");

            foreach (Type type in unfoundedTypes) 
                sb.Append(type.FullName + "\n");

            return sb.ToString();
        }

        #endregion
    }
}
