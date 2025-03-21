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
    public abstract class ModuleMonoBehaviour : MonoBehaviour, IModule, IModuleContainer
    {
        private Context _currentContext;

        private IResolver _resolver;
        private IDIRegistrationSystem _dIRegistrationSystem;

        private HashSet<Type> _allProvidedContracts;
        private IEnumerable<Type> _allDependences;

        private ChildModulesCollection _moduleCollection;
        private ChildModulesCollection _moduleCollectionProperty
        {
            get
            {
                if (_moduleCollection == null)
                    _moduleCollection = new(transform);

                return _moduleCollection;
            }
        }

        private IDisposable _tickDisposable;
        private IDisposable _fixedTickDisposable;

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

            RegisterInGameCycle();
        }

        private void InitializeChildren()
        {
            SetupChildren(new ChildSelector(_moduleCollectionProperty));

            _moduleCollectionProperty.InitializeChildren(_currentContext);
        }

        IEnumerable<IModule> IModuleContainer.GetModules() => new IModule[] { this };

        private void ResolveBindings()
        {
            _resolver = Context.Global.Resolve<IResolver>();

            _resolver.Resolve(_currentContext, this);
        }

        private void InstallBindings()
        {
            _dIRegistrationSystem.ActivateBindings(_currentContext, true);
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

        #endregion

        #region CollectionElementAPI

        public virtual HashSet<Type> GetAllProvidedContracts()
        {
            SetDIRegistrationSystem();

            if (_allProvidedContracts == null)
                _allProvidedContracts = new(
                    _moduleCollectionProperty
                    .SelectMany(m => m.GetAllProvidedContracts())
                    .Union(_dIRegistrationSystem.GetContracts()));

            return _allProvidedContracts;
        }

        public HashSet<Type> GetNecessaryDependencesInCurrentContext(Context parentContext, IEnumerable<IModule> parentChildrens)
        {
            IResolvingChecker resolvingChecker = Context.Global.Resolve<IResolvingChecker>();

            if (_allDependences == null)
                _allDependences = resolvingChecker.GetAllDependences(parentContext, GetType());

            if (resolvingChecker.CheckResolving(parentContext, _allDependences, out HashSet<Type> unresolvableTypes))
                return new(_moduleCollectionProperty
                    .SelectMany(m => m.GetNecessaryDependencesInCurrentContext(parentContext, _moduleCollectionProperty)));

            IEnumerable<Type> contractsInContext = parentChildrens
                .SelectMany(m => m.GetAllProvidedContracts());

            IEnumerable<Type> remainingTypes = unresolvableTypes.Except(contractsInContext);

            if (remainingTypes.Count() > 0)
                throw new Exception(CreateTypeNotFoundErrorLog(remainingTypes));

            return new(_moduleCollectionProperty
                .SelectMany(m => m.GetNecessaryDependencesInCurrentContext(parentContext, _moduleCollectionProperty))
                .Union(unresolvableTypes.Intersect(contractsInContext, new ReferenceComparer<Type>())));
        }

        #endregion

        #region SubtypesInterface
        
        protected virtual HashSet<Type> GetBindWaitings() => new();

        protected virtual void RegisterEvents(EventContainer eventContainer) { }
        protected virtual void IntallBindings(IDIRegistratorAPI diRegistrator) { }

        protected virtual void SetupChildren(ChildSelector childSelector) { }

        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }

        protected virtual void Dispose() { }

        #endregion

        void IDisposable.Dispose()
        {
            _moduleCollectionProperty.Dispose();

            _tickDisposable.Dispose();
            _fixedTickDisposable.Dispose();

            Dispose();
        }

        private void RegisterInGameCycle()
        {
            ITickRegistrator tickRegistrator = _currentContext.ResolveDeep<ITickRegistrator>();

            _tickDisposable = tickRegistrator.RegisterTickable(gameObject);
            _fixedTickDisposable = tickRegistrator.RegisterFixedTickable(gameObject);
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
