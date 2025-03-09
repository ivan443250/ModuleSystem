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

        private List<IModule> _children;

        private HashSet<Type> _allProvidedContrcts;

        private IEnumerable<Type> _allDependences;

        #region Initialization

        public void Construct(Context context)
        {
            _currentContext = context;

            PreInitialize();

            AddBindWaitings();

            RegisterEvents(_currentContext.EventContainer);

            SetDIRegistrationSystem();

            InstallBindings();

            InitializeChildren();

            ResolveBindings();

            Initialize();
        }

        public virtual HashSet<Type> GetAllProvidedContracts()
        {
            SetDIRegistrationSystem();

            if (_allProvidedContrcts == null)
                _allProvidedContrcts = new(
                    GetChildren()
                    .SelectMany(m => m.GetAllProvidedContracts())
                    .Union(_dIRegistrationSystem.GetContracts()));

            return _allProvidedContrcts;
        }

        public HashSet<Type> GetNecessaryDependencesInCurrentContext(Context parentContext, IEnumerable<IModule> parentChildrens)
        {
            IResolvingChecker resolvingChecker = Context.Global.Resolve<IResolvingChecker>();

            if (_allDependences == null)
                _allDependences = resolvingChecker.GetAllDependences(parentContext, GetType());

            if (resolvingChecker.CheckResolving(parentContext, _allDependences, out HashSet<Type> unresolvableTypes))
                return new(_children.SelectMany(m => m.GetNecessaryDependencesInCurrentContext(parentContext, GetChildren())));

            IEnumerable<Type> contractsInContext = parentChildrens
                .SelectMany(m => m.GetAllProvidedContracts());

            IEnumerable<Type> remainingTypes = unresolvableTypes.Except(contractsInContext);

            if (remainingTypes.Count() > 0)
                throw new Exception(CreateTypeNotFoundErrorLog(remainingTypes));

            return new(_children
                .SelectMany(m => m.GetNecessaryDependencesInCurrentContext(parentContext, GetChildren()))
                .Union(unresolvableTypes.Intersect(contractsInContext, new ReferenceComparer<Type>())));
        }

        #endregion

        #region Subtypes interface

        protected virtual HashSet<Type> GetBindWaitings() => new();

        protected virtual void RegisterEvents(EventContainer eventContainer) { }
        protected virtual void IntallBindings(IDIRegistratorAPI diRegistrator) { }
        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }

        #endregion

        private void InitializeChildren()
        {
            List<IModule> children = GetChildren();

            if (children.Count == 0)
                return;

            if (children.Count == 1)
            {
                children.First().Construct(_currentContext.CreateNext());
                return;
            }

            DependencyCollectionInitializator<IModule> initializator = new(children,
                m => m.Construct(_currentContext.CreateNext()),
                m => m.GetNecessaryDependencesInCurrentContext(_currentContext, GetChildren()));

            initializator.Initialize();
        }

        private List<IModule> GetChildren()
        {
            if (_children == null)
            {
                _children = new();

                for (int i = 0; i < transform.childCount; i++)
                    if (transform.GetChild(i).TryGetComponent(out IModule module))
                        _children.Add(module);
            }

            return _children;
        }

        private void ResolveBindings()
        {
            _resolver = Context.Global.Resolve<IResolver>();

            _resolver.Resolve(_currentContext, this);
        }

        private void InstallBindings()
        {
            _dIRegistrationSystem.ActivateBindings(_currentContext);
        }

        private void SetDIRegistrationSystem()
        {
            if (_dIRegistrationSystem != null)
                return;

            _dIRegistrationSystem = Context.Global.Resolve<IDIRegistrationSystem>();

            IntallBindings(_dIRegistrationSystem);
            _dIRegistrationSystem.StopRegistration();
        }

        private void AddBindWaitings()
        {
            IEnumerable<Type> bindWaitings = GetBindWaitings()
                .Union(GetComponents<BindWaitingRegistrator>().SelectMany(r => r.GetBindWaitings()));

            foreach (Type type in bindWaitings)
                _currentContext.AddBindWaiting(type);
        }

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
