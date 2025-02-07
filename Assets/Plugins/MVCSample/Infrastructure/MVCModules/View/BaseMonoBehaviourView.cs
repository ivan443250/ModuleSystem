using UnityEngine;

namespace MVCSample.Infrastructure
{
    public abstract class BaseMonoBehaviourView : MonoBehaviour
    {
        protected BaseModel _baseModel;

        public void Construct(BaseModel baseModel)
        {
            _baseModel = baseModel;
        }

        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }
    }
}
