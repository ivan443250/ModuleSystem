using MVCSample.Infrastructure.EventSystem;
using MVCSample.Tools;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    public class ModuleEntryPointMono : MonoBehaviour, IModule
    {
        [SerializeField] private BaseMonoBehaviourView _view;

        [SerializeField] private SerializableType _modelType;
        [SerializeField] private SerializableType _viewType;
        [SerializeField] private SerializableType _controllerType;

        public void Initialize(Context context)
        {
            InitializeChildren(context);

            RegisterEvents(context.EventContainer);
        }

        private void InitializeChildren(Context context)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                if (child.TryGetComponent(out IModule moduleComponent))
                    moduleComponent.Initialize(context.CreateNext());
            }
        }

        protected virtual void RegisterEvents(EventContainer eventContainer) { }
    }
}
