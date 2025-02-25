using UnityEngine.SceneManagement;

namespace MVCSample.Infrastructure
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