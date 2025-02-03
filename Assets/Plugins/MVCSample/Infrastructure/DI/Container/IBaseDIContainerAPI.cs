namespace MVCSample.Infrastructure.DI
{
    public interface IBaseDIContainerAPI
    {
        void Register<TInterface, TImplementation>(bool asSingle = true) where TImplementation : TInterface;
        void RegisterInstance<TInterface>(TInterface instance);
        T Resolve<T>();
        bool TryResolve<T>(out T instance) where T : class;
        bool HasBinding<T>();
    }
}
