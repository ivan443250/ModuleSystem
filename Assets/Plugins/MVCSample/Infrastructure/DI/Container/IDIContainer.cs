using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public interface IDIContainer
    {
        IBaseDIContainerAPI GetContainerAPI();
        void AddInstallers(IEnumerable<IInstaller> installers);
    }
}