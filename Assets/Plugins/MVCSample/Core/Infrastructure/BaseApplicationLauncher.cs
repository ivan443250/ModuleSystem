using UnityEngine;

namespace MVCSample.Infrastructure
{
    public class BaseApplicationLauncher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutostartApplication()
        {
            
        }
    }
}