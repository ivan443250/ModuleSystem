using System.Collections;
using UnityEngine;

namespace MVCSample.Tools
{
    public interface ICorutineStarter
    {
        Coroutine Start(IEnumerator coroutine);
        void Stop(Coroutine coroutine);
    }
}
