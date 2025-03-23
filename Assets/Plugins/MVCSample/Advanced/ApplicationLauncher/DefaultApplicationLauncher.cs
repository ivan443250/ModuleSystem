using MVCSample.Advanced;
using MVCSample.Infrastructure.DI;
using MVCSample.SceneManagement;
using MVCSample.SceneManagement.Test;

namespace MVCSample.Infrastructure
{
    public class DefaultApplicationLauncher : BaseApplicationLauncher
    {
        protected override ISystemBindInstaller GetSystemBindInstaller()
        {
            return new ZenjectSystemBindInstaller();
        }

        protected override void Initialize()
        {
            ISceneManager sceneManager = Context.Global.Resolve<ISceneManager>();
            SceneData sceneData = new TestSceneData("TestScene", () => GlobalContext.CreateNext());
            sceneManager.AddScene(sceneData);

            Context.Global.Resolve<ISceneLoader>().Load("TestScene");
        }
    }
}