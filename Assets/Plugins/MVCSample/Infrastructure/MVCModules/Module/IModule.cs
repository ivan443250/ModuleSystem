using UnityEngine;

namespace MVCSample.Infrastructure
{
    public interface IModule
    {
        public void Initialize(Context context);
    }
}