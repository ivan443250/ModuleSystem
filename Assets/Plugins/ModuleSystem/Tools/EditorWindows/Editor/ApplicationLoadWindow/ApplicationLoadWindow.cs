using MVCSample.Tools.Constants;
using MVCSample.Tools.DataContainers;
using UnityEditor;

namespace MVCSample.Tools.EditorWindows
{
    public class ApplicationLoadWindow : EditorWindow
    {
        private static bool _isOpen = false;

        private SerializedObject _loadDataContainerSerialized;
        private SerializedProperty _applicationLauncherTypeSerialized;

        [MenuItem("Window/ApplicationLoadWindow")]
        static void OpenWindow()
        {
            if (_isOpen)
                throw new System.Exception("ApplicationLoadWindow is already open");

            _isOpen = true;
            GetWindow(typeof(ApplicationLoadWindow)).Show();
        }

        private void OnEnable()
        {
            var loadDataContainer = AssetDatabase
                .LoadAssetAtPath<ApplicationLoadDataContainer>(AssetDatabaseDataPathConstants.ApplicationLoadDataPath);

            _loadDataContainerSerialized = new SerializedObject(loadDataContainer);
            _applicationLauncherTypeSerialized = _loadDataContainerSerialized.FindProperty(ApplicationLoadDataContainer.LauncherTypeField);
        }

        private void OnGUI()
        {
            EditorGUILayout.PropertyField(_applicationLauncherTypeSerialized);
            _loadDataContainerSerialized.ApplyModifiedProperties();
        }

        private void OnDestroy()
        {
            _isOpen = false;
        }
    }
}
