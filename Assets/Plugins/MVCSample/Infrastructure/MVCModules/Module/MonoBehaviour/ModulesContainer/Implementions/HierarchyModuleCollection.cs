using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    public class HierarchyModuleCollection : MonoBehaviour, IModuleContainer
    {
        public IEnumerable<IModule> GetModules()
        {
            List<IModuleContainer> childContainers = new();

            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).TryGetComponent(out IModuleContainer container))
                    childContainers.Add(container);

            return childContainers.SelectMany(container => container.GetModules());
        }
    }
}
