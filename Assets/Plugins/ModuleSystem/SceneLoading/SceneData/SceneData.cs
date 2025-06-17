using MVCSample.Infrastructure;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace MVCSample.SceneManagement
{
    [Serializable]
    public abstract class SceneData
    {
        public string Name => _name;

        [SerializeField] private string _name;
        [SerializeField] private int _buildIndex;

        private Func<Context> _getContextCallback;

        private SceneData(Func<Context> getContextCallback) 
        {
            _getContextCallback = getContextCallback;
        }

        public SceneData(string name, Func<Context> getContextCallback) : this(getContextCallback)
        {
            _name = name;

            _buildIndex = SceneManager.GetSceneByName(_name).buildIndex;
        }

        public SceneData(int buildIndex, Func<Context> getContextCallback) : this(getContextCallback)
        {
            _buildIndex = buildIndex;

            _name = SceneManager.GetSceneByBuildIndex(_buildIndex).name;
        }

        public Context CreateSceneContext()
        {
            return _getContextCallback.Invoke();
        }
    }
}
