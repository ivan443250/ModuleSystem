using MVCSample.Tools.Constants;
using MVCSample.Tools.DataContainers;
using System;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    public class ApplicationLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutostartApplication()
        {
            if (GetApplicationLauncher() is not BaseApplicationLauncher applicationLauncher)
                throw new InvalidOperationException();

            applicationLauncher.Launch();
        }

        private static object GetApplicationLauncher()
        {
            var loadDataContainer = Resources.Load<ApplicationLoadDataContainer>(ResourcesDataPathConstants.ApplicationLoadDataPath);

            return Activator.CreateInstance(loadDataContainer.GetLauncherType());
        }
    }
}
