using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCSample.Tools
{
    public sealed class TypeDictionary<ValueT>
    {
        private Dictionary<Type, ValueT> _dictionary;

        public ValueT this[Type type] => _dictionary[type];

        #region Constructors
        public TypeDictionary()
        {
            _dictionary = new();
        }

        public TypeDictionary(IEnumerable<KeyValuePair<Type, ValueT>> pairs)
        {
            _dictionary = new(pairs);
        }

        public TypeDictionary(params (Type type, ValueT value)[] values)
        {
            KeyValuePair<Type, ValueT>[] keyValuePairs = values
                .Select((pair) => new KeyValuePair<Type, ValueT>(pair.type, pair.value))
                .ToArray();

            _dictionary = new(keyValuePairs);
        }
        #endregion

        #region Add
        public void Add<T>(ValueT value, bool throwEx = false)
        {
            Add(typeof(T), value, throwEx);
        }

        public void Add(Type representType, ValueT value, bool throwEx = false)
        {
            if (_dictionary.ContainsKey(representType) == false)
                _dictionary.Add(representType, value);
            else if (throwEx)
                throw new ArgumentException();
            else
                _dictionary[representType] = value;
        }
        #endregion

        #region Remove
        public void Remove<T>(bool throwEx = false)
        {
            Remove(typeof(T), throwEx);
        }

        public void Remove(Type type, bool throwEx = false)
        {
            if (_dictionary.ContainsKey(type))
                _dictionary.Remove(type);
            else if (throwEx)
                throw new ArgumentException();
        }
        #endregion
    }
}