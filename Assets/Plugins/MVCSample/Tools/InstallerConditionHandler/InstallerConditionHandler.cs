using MVCSample.Infrastructure.DI;
using UnityEngine;

namespace MVCSample.Tools
{
    public struct InstallerConditionHandler<TContainer>
    {
        private readonly TContainer _container;
        private readonly IBaseDIContainerAPI _containerAPI;

        public InstallerConditionHandler(TContainer container, IBaseDIContainerAPI containerAPI)
        {
            _container = container;
            _containerAPI = containerAPI;
        }

        public void HandleInstallerConditions(IInstaller installer)
        {
            bool isUsed = false;

            if (CheckGenericInstaller(installer))
                isUsed = true;

            if (CheckBaseAPIInstaller(installer))
                isUsed = true;

            if (isUsed == false) 
                Debug.LogWarning($"Installer of type {installer.GetType()} not used");
        }

        private bool CheckGenericInstaller(IInstaller installer)
        {
            if (installer is not IGenericInstaller<TContainer> genericInstaller)
                return false;

            genericInstaller.InstallBindings(_container);

            return true;
        }

        private bool CheckBaseAPIInstaller(IInstaller installer)
        {
            if (installer is not IBaseAPIInstaller apiInstaller)
                return false;

            apiInstaller.InstallBindings(_containerAPI);

            return true;
        }
    }
}
