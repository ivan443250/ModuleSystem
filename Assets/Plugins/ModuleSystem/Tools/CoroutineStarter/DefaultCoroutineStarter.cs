using System.Collections;
using UnityEngine;

namespace MVCSample.Tools
{
    public class DefaultCoroutineStarter : ICorutineStarter
    {
        private readonly CoroutineProvider _coroutineProvider;

        public DefaultCoroutineStarter()
        {
            _coroutineProvider = new GameObject("CoroutineProvider").AddComponent<CoroutineProvider>();
            Object.DontDestroyOnLoad(_coroutineProvider);
        }

        public Coroutine Start(IEnumerator routine)
        {
            return _coroutineProvider.StartCoroutine(routine);
        }

        public void Stop(Coroutine coroutine)
        {
            _coroutineProvider.StopCoroutine(coroutine);
        }
    }

    public class CoroutineProvider : MonoBehaviour { }
}