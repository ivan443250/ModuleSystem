using UnityEngine;

namespace MVCSample.SceneManagement
{
    public interface ISceneLoader
    {
        void Load(string sceneName);
        void Load(int sceneIndex);
    }
}