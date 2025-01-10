using System;
using UnityEditor;
using UnityEngine;

namespace MVCSample.Tools
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypePropertyDrawer : PropertyDrawer
    {
        private bool _isInitialized;

        SerializedProperty _classNameProperty;
        SerializedProperty _scriptProperty;

        public SerializableTypePropertyDrawer()
        {
            _isInitialized = false;
        }

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            if (_isInitialized == false)
                Initialize(mainProperty);

            UnityEngine.Object scriptObj = _scriptProperty.objectReferenceValue;
            scriptObj = EditorGUI.ObjectField(position, label, scriptObj, typeof(MonoScript), true);

            SetClassName(mainProperty, scriptObj);
        }

        private void SetClassName(SerializedProperty mainProperty, UnityEngine.Object scriptObj)
        {
            if (scriptObj is not MonoScript script)
                return;

            Type type = script.GetClass();

            _scriptProperty.objectReferenceValue = script;
            _classNameProperty.stringValue = $"{type.FullName}, {type.Assembly.FullName}";
        }

        private void Initialize(SerializedProperty mainProperty)
        {
            _isInitialized = true;

            _classNameProperty = mainProperty.FindPropertyRelative(SerializableType.ClassNameField);
            _scriptProperty = mainProperty.FindPropertyRelative(SerializableType.ScriptField);
        }
    }
}
