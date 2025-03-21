using System;

namespace MVCSample.Infrastructure
{
    public struct ChildSetuper
    {
        private ChildModulesCollection _collection;

        private Predicate<IModule> _selectionCallback;

        public ChildSetuper(ChildModulesCollection collection, Predicate<IModule> selectionCallback)
        {
            _collection = collection;

            _selectionCallback = selectionCallback;
        }

        public void Setup(Action<IModule, Context> initializationCallback)
        {
            _collection.SetupChildren(_selectionCallback, initializationCallback);
        }
    }
}