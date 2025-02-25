using UnityEngine;

namespace MVCSample.Infrastructure
{
    public interface ISceneLoader
    {
        void Load(string sceneName);
        void Load(int sceneIndex);
    }
}