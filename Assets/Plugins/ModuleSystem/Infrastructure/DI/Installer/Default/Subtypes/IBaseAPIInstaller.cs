namespace MVCSample.Infrastructure.DI
{
    public interface IBaseAPIInstaller : IInstaller
    {
        void InstallBindings(IBaseDIContainerAPI container);
    }
}
