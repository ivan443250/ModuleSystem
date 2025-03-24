using MVCSample.Tools;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVCSample.SceneManagement
{
    public abstract class BaseSceneLoader : ISceneLoader
    {
        private SceneEntryPoint _currentEntryPoint;

        private ICorutineStarter _coroutineStarter;

        protected BaseSceneLoader(ICorutineStarter coroutineStarter) 
        {
            _coroutineStarter = coroutineStarter;
        }

        public void Load(string sceneName)
        {
            _coroutineStarter.Start(ChangeScene(sceneName));
        }

        public void Load(int sceneIndex)
        {
            _coroutineStarter.Start(ChangeScene(SceneManager.GetSceneByBuildIndex(sceneIndex).name));
        }

        protected abstract IEnumerator LoadInternal(string sceneName);

        private IEnumerator ChangeScene(string sceneName)
        {
            if (_currentEntryPoint == null)
            {
                yield return null;
                ActivateCurrentScene();
                yield break;
            }

            if (SceneManager.GetActiveScene().name == sceneName)
            {
                Debug.LogWarning("Attempt to change scene on the same scene");
                yield break;
            }

            _currentEntryPoint.DisposeSceneModules();

            yield return _coroutineStarter.Start(LoadInternal(sceneName));

            ActivateCurrentScene();
        }

        private void ActivateCurrentScene()
        {
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            if (rootGameObjects.Length == 0)
            {
                Debug.LogWarning("Initialized empty scene");
                return;
            }

            if (rootGameObjects.First().TryGetComponent(out SceneEntryPoint entryPoint) == false)
                throw new System.Exception(CreateLogText());

            _currentEntryPoint = entryPoint;
            _currentEntryPoint.ActivateSceneModules();
        }

        private string CreateLogText()
        {
            return $"Scene {SceneManager.GetActiveScene().name} can not contain GameObjects in MainRoot";
        }
    }
}
