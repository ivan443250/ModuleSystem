using MVCSample.Tools.Constants;
using MVCSample.Tools.DataContainers;
using System;
using UnityEditor;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    public class ApplicationLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutostartApplication()
        {
            Debug.Log(GetApplicationLauncherType().FullName);
        }

        private static Type GetApplicationLauncherType()
        {
            var loadDataContainer = Resources.Load<ApplicationLoadDataContainer>(ResourcesDataPathConstants.ApplicationLoadDataPath);

            return loadDataContainer.GetLauncherType();
        }
    }
}
