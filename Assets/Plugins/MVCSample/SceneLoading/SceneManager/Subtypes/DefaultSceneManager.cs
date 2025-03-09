using System;
using System.Collections.Generic;

namespace MVCSample.SceneManagement
{
    public class DefaultSceneManager : ISceneManager
    {
        private Dictionary<string, SceneData> _nameSceneDataPairs;

        public DefaultSceneManager()
        {
            _nameSceneDataPairs = new();
        }

        public SceneData GetSceneData(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
                throw new ArgumentNullException("0");

            if (_nameSceneDataPairs.ContainsKey(sceneName) == false)
                throw new ArgumentException("1");

            return _nameSceneDataPairs[sceneName];
        }

        public void AddScene(SceneData data, bool throwEx = true)
        {
            if (_nameSceneDataPairs.ContainsKey(data.Name) == false)
            {
                _nameSceneDataPairs.Add(data.Name, data);
                return;
            }

            if (throwEx)
                throw new Exception();

            _nameSceneDataPairs[data.Name] = data;
        }
    }
}
