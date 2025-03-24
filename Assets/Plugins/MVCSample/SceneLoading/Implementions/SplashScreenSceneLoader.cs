using MVCSample.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVCSample.SceneManagement
{
    public class SplashScreenSceneLoader : BaseSceneLoader
    {
        private GameObject _splashScreen;

        public SplashScreenSceneLoader(ICorutineStarter corutineStarter) : base(corutineStarter)
        {
            GameObject prefab = Resources.Load<GameObject>("SplashScreen");

            _splashScreen = Object.Instantiate(prefab);

            Object.DontDestroyOnLoad(_splashScreen);

            _splashScreen.gameObject.SetActive(false);
        }

        protected override IEnumerator LoadInternal(string sceneName)
        {
            _splashScreen.gameObject.SetActive(true);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }

            asyncLoad.allowSceneActivation = true;
            yield return new WaitUntil(() => asyncLoad.isDone);
            _splashScreen.gameObject.SetActive(false);
        }
    }
}
