using UnityEngine.SceneManagement;

namespace MVCSample.SceneManagement
{
    public class DefaultSceneLoader : ISceneLoader
    {
        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void Load(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}