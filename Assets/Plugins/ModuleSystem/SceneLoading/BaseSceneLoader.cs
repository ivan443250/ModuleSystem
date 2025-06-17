using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using MVCSample.Infrastructure.DataHolding;
using MVCSample.SceneManagement;
using MVCSample.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins.MVCSample.SceneLoading
{
    public abstract class BaseSceneLoader : ISceneLoader
    {
        private SceneEntryPoint _currentEntryPoint;

        private readonly ICorutineStarter _coroutineStarter;

        private readonly IDataExplorer _dataExplorer;

        protected BaseSceneLoader(ICorutineStarter coroutineStarter, IDataExplorer dataExplorer) 
        {
            _coroutineStarter = coroutineStarter;

            _dataExplorer = dataExplorer;
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
                if (_currentEntryPoint == null)
                {
                    yield return _coroutineStarter.Start(ActivateCurrentScene());
                    yield break;
                }

                Debug.LogWarning("Attempt to change scene on the same scene");
                yield break;
            }

            yield return WaitForTask(_currentEntryPoint?.DisposeSceneModules());

            yield return _coroutineStarter.Start(LoadInternal(sceneName));

            yield return _coroutineStarter.Start(ActivateCurrentScene());
        }

        private IEnumerator ActivateCurrentScene()
        {
            yield return WaitForTask(_dataExplorer.OpenSceneDataSet(SceneManager.GetActiveScene().name));

            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            if (rootGameObjects.Length == 0)
            {
                Debug.LogWarning("Initialized empty scene");
                yield break;
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

        private IEnumerator WaitForTask(Task task)
        {
            if (task == null)
                yield break;

            while (!task.IsCompleted)
            {
                yield return null;
            }
        }
    }
}
