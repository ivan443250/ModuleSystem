using MVCSample.Advanced;
using MVCSample.Infrastructure.DI;
using MVCSample.SceneManagement;

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
            Context.Global.Resolve<ISceneLoader>().Load("TestScene");
        }
    }
}