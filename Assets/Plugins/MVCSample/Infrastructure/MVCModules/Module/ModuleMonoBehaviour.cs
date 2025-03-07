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
    public abstract class ModuleMonoBehaviour : MonoBehaviour, IModule
    {
        private Context _currentContext;

        private IResolver _resolver;

        private IDIRegistrationSystem _dIRegistrationSystem;

        #region Initialization

        public void Construct(Context context)
        {
            Debug.Log($"const start {GetType()}");

            _currentContext = context;

            PreInitialize();

            RegisterEvents(_currentContext.EventContainer);

            SetDIRegistrationSystem();

            InstallBindings();

            InitializeChildren(_currentContext);

            ResolveBindings();

            Initialize();

            Debug.Log($"const end {GetType()}");
        }

        public virtual HashSet<Type> GetAllProvidedContracts()
        {
            SetDIRegistrationSystem();

            return _dIRegistrationSystem.GetContracts();
        }

        public HashSet<Type> GetNecessaryDependencesInCurrentContext(Context parentContext)
        {
            IResolvingChecker resolvingChecker = Context.Global.Resolve<IResolvingChecker>();

            if (resolvingChecker.CheckResolving(parentContext, GetType(), out HashSet<Type> unresolvableTypes))
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
        protected virtual void IntallBindings(IDIRegistratorAPI diRegistrator) { }
        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }

        #endregion

        #region Inside methods

        private void InitializeChildren(Context context)
        {
            List<IModule> children = GetChildren();

            if (children.Count == 0)
                return;

            if (children.Count == 1)
            {
                children.First().Construct(context.CreateNext());
                return;
            }

            DependencyCollectionInitializator<IModule> initializator = new(children, 
                m => m.Construct(context.CreateNext()),
                m => m.GetNecessaryDependencesInCurrentContext(context));

            initializator.Initialize();
        }

        private List<IModule> GetChildren()
        {
            return GetChildren(transform);
        }

        private List<IModule> GetChildren(Transform currentTransform)
        {
            List<IModule> children = new();

            for (int i = 0; i < currentTransform.childCount; i++)
                if (currentTransform.GetChild(i).TryGetComponent(out IModule module))
                    children.Add(module);

            return children;
        }

        private void ResolveBindings()
        {
            _resolver = Context.Global.Resolve<IResolver>();

            _resolver.Resolve(_currentContext, this);
        }

        private void InstallBindings()
        {
            _dIRegistrationSystem.ActivateBindings(_currentContext.DiContainer.ContainerAPI);
        }

        private void SetDIRegistrationSystem()
        {
            if (_dIRegistrationSystem != null)
                return;

            _dIRegistrationSystem = Context.Global.Resolve<IDIRegistrationSystem>();

            IntallBindings(_dIRegistrationSystem);
            _dIRegistrationSystem.StopRegistration();
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
