using MVCSample.Infrastructure.EventSystem;
using MVCSample.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    public class ModuleEntryPointMono : MonoBehaviour, IModule
    {
        [SerializeField] private BaseMonoBehaviourView _view;

        [SerializeField] private SerializableType _modelType;
        [SerializeField] private SerializableType _controllerType;

        public HashSet<Type> GetNecessaryDependencyTypes()
        {
            throw new NotImplementedException();
        }

        public void Initialize(Context context)
        {
            InitializeChildren(context);

            RegisterEvents(context.EventContainer);
        }

        public bool TryGetContract<T>(out T contract)
        {
            throw new NotImplementedException();
        }

        private void InitializeChildren(Context context)
        {
            List<Transform> children = new();

            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
        }

        protected virtual void RegisterEvents(EventContainer eventContainer) { }
    }
}
