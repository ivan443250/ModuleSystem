namespace MVCSample.Infrastructure.DI
{
    public interface IGenericInstaller<TContainer> : IInstaller
    {
        void InstallBindings(TContainer container);
    }
}
