using MVCSample.Infrastructure;
using MVCSample.Infrastructure.DI;

namespace MVCSample.SceneManagement
{
    public sealed class SceneEntryPoint : ModuleMonoBehaviour
    {
        private IModuleRegistrator _moduleRegistrator;

        public void ActivateSceneModules()
        {
            _moduleRegistrator = Context.Global.Resolve<IModuleRegistrator>();

            Construct(Context.CreateNewInGlobal());
        }

        public void DisposeSceneModules()
        {
            _moduleRegistrator.Dispose();

            IModule module = this;
            module.Dispose();
        }

        protected override void IntallBindings(IDIRegistratorAPI diRegistrator)
        {
            diRegistrator.RegisterInstance(_moduleRegistrator);
        }
    }
}
