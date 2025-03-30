using MVCSample.Infrastructure;
using MVCSample.Infrastructure.DataHolding;
using MVCSample.Infrastructure.DI;
using MVCSample.SceneManagement;
using MVCSample.Tools;

namespace MVCSample.Advanced
{
    public struct ZenjectSystemBindInstaller : IGenericInstaller<Zenject.DiContainer>, ISystemBindInstaller
    {
        public IDIContainer InstallSystemBindings()
        {
            ZenjectContainerFacade containerFacade = new();

            containerFacade.AddInstallers(new IInstaller[] { this });

            return containerFacade;
        }

        public void InstallBindings(Zenject.DiContainer container)
        {
            container
                .Bind<IDIContainer>()
                .To<ZenjectContainerFacade>()
                .AsTransient();

            container
                .Bind<ICorutineStarter>()
                .To<DefaultCoroutineStarter>()
                .AsSingle();

            container
                .Bind(typeof(IResolver), typeof(IResolvingChecker))
                .To<GenericResolver>()
                .AsSingle();

            container
                .Bind<ISceneLoader>()
                .To<DefaultSceneLoader>()
                .AsSingle();

            container
                .Bind<IDIRegistrationSystem>()
                .To<DIInstallerSystem>()
                .AsTransient();

            container
                .Bind<IRegistrator>()
                .To<GameCycleRegistrator>()
                .AsTransient();

            container
                .Bind<IFileSystem>()
                .To<DefaultFileSystem>()
                .AsTransient();

            container
                .Bind<IDataConverter>()
                .To<DefaultDataConverter>()
                .AsTransient();

            container
                .Bind<IModuleRegistrator>()
                .To<SceneModuleRegistrator>()
                .AsTransient();

            container
                .Bind<IGameLoop>()
                .To<GameLoop>()
                .AsSingle();

            container
                .Bind<IRegistrator>()
                .To<DataReaderRegistrator>()
                .AsTransient();

            container
               .Bind<IDataExplorer>()
               .To<DefaultDataExplorer>()
               .AsSingle();
        }
    }
}