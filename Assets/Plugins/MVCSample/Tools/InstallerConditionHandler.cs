using MVCSample.Infrastructure.DI;
using UnityEngine;

namespace MVCSample.Tools
{
    public struct InstallerConditionHandler
    {
        public static void HandleInstallerConditions<TContainer>(IInstaller installer, TContainer container, IBaseDIContainerAPI api)
        {
            bool isUsed = false;

            if (CheckGenericInstaller(installer, container))
                isUsed = true;

            if (CheckBaseAPIInstaller(installer, api))
                isUsed = true;

            if (isUsed == false) 
                Debug.LogWarning($"Installer of type {installer.GetType()} not used");
        }

        private static bool CheckGenericInstaller<TContainer>(IInstaller installer, TContainer container)
        {
            if (installer is not IGenericInstaller<TContainer> genericInstaller)
                return false;

            genericInstaller.InstallBindings(container);

            return true;
        }

        private static bool CheckBaseAPIInstaller(IInstaller installer, IBaseDIContainerAPI api)
        {
            if (installer is not IBaseAPIInstaller apiInstaller)
                return false;

            apiInstaller.InstallBindings(api);

            return true;
        }
    }
}
