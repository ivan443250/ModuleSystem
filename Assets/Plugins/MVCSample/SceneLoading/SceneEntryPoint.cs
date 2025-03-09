using MVCSample.Infrastructure;
using UnityEngine.SceneManagement;

namespace MVCSample.SceneManagement
{
    public class SceneEntryPoint : ModuleMonoBehaviour
    {
        private void Awake()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            SceneData sceneData = Context.Global
                .Resolve<ISceneManager>()
                .GetSceneData(sceneName);

            Construct(sceneData.Context);
        }
    }
}
