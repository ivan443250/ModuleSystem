using MVCSample.Infrastructure;
using System;
using UnityEngine.SceneManagement;

namespace MVCSample.SceneManagement
{
    public abstract class SceneData
    {
        public string Name { get; private set; }
        public int BuildIndex { get; private set; }

        public Func<Context> _getContextCallback;

        private SceneData(Func<Context> getContextCallback) 
        {
            _getContextCallback = getContextCallback;
        }

        public SceneData(string name, Func<Context> getContextCallback) : this(getContextCallback)
        {
            Name = name;

            BuildIndex = SceneManager.GetSceneByName(Name).buildIndex;
        }

        public SceneData(int buildIndex, Func<Context> getContextCallback) : this(getContextCallback)
        {
            BuildIndex = buildIndex;

            Name = SceneManager.GetSceneByBuildIndex(BuildIndex).name;
        }

        public Context CreateSceneContext()
        {
            return _getContextCallback.Invoke();
        }
    }
}
