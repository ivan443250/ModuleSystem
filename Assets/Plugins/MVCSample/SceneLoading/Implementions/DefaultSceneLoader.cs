using MVCSample.Infrastructure.DataHolding;
using MVCSample.Tools;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MVCSample.SceneManagement
{
    public class DefaultSceneLoader : BaseSceneLoader
    {
        public DefaultSceneLoader(ICorutineStarter corutineStarter, IDataExplorer dataExplorer) : base(corutineStarter, dataExplorer) { }

        protected override IEnumerator LoadInternal(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            yield break;
        }
    }
}