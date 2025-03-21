using MVCSample.Infrastructure;
using MVCSample.Infrastructure.DI;
using MVCSample.Tools;
using UnityEngine;

namespace MVCSample.SceneManagement
{
    public sealed class SceneEntryPoint : ModuleMonoBehaviour
    {
        private IGameCycle _gameCycle;

        public void ActivateSceneModules(SceneData sceneData)
        {
            _gameCycle = Context.Global.Resolve<IGameCycle>();

            Construct(sceneData.CreateSceneContext());
        }

        public void DisposeSceneModules()
        {
            IModule module = this;
            module.Dispose();
        }

        protected override void IntallBindings(IDIRegistratorAPI diRegistrator)
        {
            diRegistrator.RegisterInstance<ITickRegistrator>(_gameCycle);
        }

        private void Update()
        {
            if (_gameCycle == null)
                return;

            _gameCycle.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_gameCycle == null)
                return;

            _gameCycle.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}
