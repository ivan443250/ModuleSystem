using MVCSample.Infrastructure;
using MVCSample.Infrastructure.DI;
using System.Threading.Tasks;

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

        public async Task DisposeSceneModules()
        {
            await _moduleRegistrator.DisposeAsync();

            IModule module = this;
            module.Dispose();
        }

        protected override void IntallBindings(IDIRegistratorAPI diRegistrator)
        {
            diRegistrator.RegisterInstance(_moduleRegistrator);
        }
    }
}
