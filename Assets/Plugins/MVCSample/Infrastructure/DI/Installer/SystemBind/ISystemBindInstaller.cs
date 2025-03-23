using UnityEngine;

namespace MVCSample.Infrastructure.DI
{
    public interface ISystemBindInstaller
    {
        IDIContainer InstallSystemBindings();
    }
}