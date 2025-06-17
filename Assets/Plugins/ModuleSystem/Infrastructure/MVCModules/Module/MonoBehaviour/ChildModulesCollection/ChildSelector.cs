using System;

namespace MVCSample.Infrastructure
{
    public struct ChildSelector
    {
        private ChildModulesCollection _collection;

        public ChildSelector(ChildModulesCollection collection)
        {
            _collection = collection;
        }

        public ChildSetuper Select(Predicate<IModule> selectionCallback)
        {
            return new ChildSetuper(_collection, selectionCallback);
        }

        public ChildSetuper SelectAll()
        {
            return new ChildSetuper(_collection, _ => true);
        }
    }
}