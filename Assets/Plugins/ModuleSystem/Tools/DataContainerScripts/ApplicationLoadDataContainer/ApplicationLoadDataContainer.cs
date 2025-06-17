using System;
using UnityEngine;

namespace MVCSample.Tools.DataContainers
{
    [CreateAssetMenu(fileName = "ApplicationLoadDataContainer", menuName = "Scriptable Objects/ApplicationLoadDataContainer")]
    public class ApplicationLoadDataContainer : ScriptableObject
    {
        public const string LauncherTypeField = nameof(_applicationLauncherType);

        [HideInInspector]
        [SerializeField]
        private SerializableType _applicationLauncherType;

        public Type GetLauncherType()
        {
            if (_applicationLauncherType == null)
                throw new NullReferenceException();

            return _applicationLauncherType.GetValue();
        }
    }
}
