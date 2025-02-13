using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public interface IDIContainer
    {
        IBaseDIContainerAPI ContainerAPI { get; }
        void AddInstallers(IEnumerable<IInstaller> installers);
    }
}