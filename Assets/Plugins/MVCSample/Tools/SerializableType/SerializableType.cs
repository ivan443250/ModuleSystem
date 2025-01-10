using System;
using UnityEditor;
using UnityEngine;

namespace MVCSample.Tools
{
    [Serializable]
    public class SerializableType
    {
#if UNITY_EDITOR
        public const string ClassNameField = nameof(_className);
        public const string ScriptField = nameof(_script);

        [SerializeField] private MonoScript _script;
#endif
        [HideInInspector]
        [SerializeField] 
        private string _className;

        public Type Value
        {
            get
            {
                if (_className == null)
                    throw new NullReferenceException();

                return Type.GetType(_className);
            }
        }
    }
}