using MVCSample.Infrastructure;
using MVCSample.Infrastructure.DI;

namespace MVCSample.SceneManagement
{
    public sealed class SceneEntryPoint : ModuleMonoBehaviour
    {
        private IModuleRegistrator _moduleRegistrator;

        public void ActivateSceneModules(SceneData sceneData)
        {
            _moduleRegistrator = Context.Global.Resolve<IModuleRegistrator>();

            Construct(sceneData.CreateSceneContext());
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
