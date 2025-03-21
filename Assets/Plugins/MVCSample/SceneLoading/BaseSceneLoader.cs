using MVCSample.Infrastructure;
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

        private ISceneManager _sceneManager;

        protected BaseSceneLoader(ICorutineStarter coroutineStarter, ISceneManager sceneManager) 
        {
            _coroutineStarter = coroutineStarter;

            _sceneManager = sceneManager;
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
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                Debug.LogWarning("Attempt to change scene on the same scene");
                yield break;
            }

            if (_currentEntryPoint != null)
                _currentEntryPoint.DisposeSceneModules();

            yield return _coroutineStarter.Start(LoadInternal(sceneName));

            if (SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .First()
                .TryGetComponent(out SceneEntryPoint entryPoint) == false)
            {
                throw new System.Exception(CreateLogText());
            }

            _currentEntryPoint = entryPoint;
            _currentEntryPoint.ActivateSceneModules(_sceneManager.GetSceneData(sceneName));
        }

        private string CreateLogText()
        {
            return $"Scene {SceneManager.GetActiveScene().name} can not contain GameObjects in MainRoot";
        }
    }
}
