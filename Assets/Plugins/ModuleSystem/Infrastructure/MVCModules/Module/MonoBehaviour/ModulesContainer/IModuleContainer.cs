using System.Collections.Generic;

namespace MVCSample.Infrastructure
{
    public interface IModuleContainer
    {
        IEnumerable<IModule> GetModules();
    }
}
