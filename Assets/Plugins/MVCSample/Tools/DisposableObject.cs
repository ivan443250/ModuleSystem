using System;

namespace MVCSample.Tools
{
    public struct DisposableObject : IDisposable
    {
        private Action _disposeCallback;

        public DisposableObject(Action disposeCallback)
        {
            _disposeCallback = disposeCallback;
        }

        public void Dispose()
        {
            _disposeCallback.Invoke();
        }

        public static DisposableObject operator +(DisposableObject obj1, DisposableObject obj2)
        {
            Action newDisposeCallback = obj1._disposeCallback;
            newDisposeCallback += obj2._disposeCallback;

            return new DisposableObject(newDisposeCallback);
        }
    }
}