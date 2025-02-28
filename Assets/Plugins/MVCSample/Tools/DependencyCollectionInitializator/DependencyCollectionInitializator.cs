using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCSample.Tools
{
    public struct DependencyCollectionInitializator<T> where T : IDependencyCollectionElement
    {
        private T[] _elementArray;
        private Dictionary<Type, int> _typeIndexPair;
        private Action<T> _initializationCallback;

        private HashSet<int> _initializedElements;
        private Stack<int> _initializeStack;

        public DependencyCollectionInitializator(IEnumerable<T> collection, Action<T> initializationCallback)
        {
            _elementArray = collection.ToArray();

            _typeIndexPair = new();

            for (int i = 0; i < _elementArray.Length; i++)
            {
                HashSet<Type> providedContracts = _elementArray[i].GetAllProvidedContracts();

                foreach (Type type in providedContracts)
                {
                    if (_typeIndexPair.ContainsKey(type))
                        throw new Exception();

                    _typeIndexPair.Add(type, i);
                }
            }

            _initializationCallback = initializationCallback;

            _initializedElements = new();
            _initializeStack = new();
        }

        #region Initialization

        public void Initialize()
        {
            foreach (Type type in _typeIndexPair.Keys)
            {
                InitializeElement(type);
            }
        }

        private void InitializeElement(Type type)
        {
            int index = _typeIndexPair[type];

            if (_initializedElements.Contains(index))
                return;

            if (_initializeStack.Contains(index))
                throw new Exception(CreateErrorLoopLog(index));

            _initializeStack.Push(index);

            HashSet<Type> uninitializedDependentTypes = _elementArray[index].GetNecessaryDependencesInCurrentContext();

            while (uninitializedDependentTypes.Count > 0)
            {
                Type dependentType = uninitializedDependentTypes.First();

                InitializeElement(dependentType);

                uninitializedDependentTypes.Remove(dependentType);
            }

            _initializationCallback.Invoke(_elementArray[index]);

            _initializedElements.Add(index);

            _initializeStack.Pop();
        }

        #endregion

        #region Logs

        private string CreateErrorLoopLog(int endIndex)
        {
            StringBuilder sb = new("infinity initialization loop was created\n");

            IEnumerable<int> initStack = _initializeStack.Reverse();

            foreach (int index in initStack)
                sb.Append($"\n{_elementArray[index].GetType()}");

            sb.Append($"\n{_elementArray[endIndex].GetType()}\n");

            return sb.ToString();
        }

        #endregion
    }
}
