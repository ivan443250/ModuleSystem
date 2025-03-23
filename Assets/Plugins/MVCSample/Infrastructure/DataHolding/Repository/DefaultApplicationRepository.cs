using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DataHolding
{
    public class DefaultApplicationRepository : IApplicationRepository
    {
        //Main root path is "" (string.Empty)
        private Dictionary<string, Dictionary<Type, object>> _savedObjects;

        public void SetDataElements(string path = "", params object[] elements)
        {
            if (path == null)
                throw new ArgumentNullException();

            if (elements.Length == 0)
                return;

            if (_savedObjects.ContainsKey(path) == false)
                _savedObjects.Add(path, new());

            foreach (object element in elements)
            {
                if (_savedObjects[path].ContainsKey(element.GetType()))
                    _savedObjects[path].Remove(element.GetType());

                _savedObjects[path].Add(element.GetType(), element);
            }
        }

        public object GetData(Type type, string path = "")
        {
            throw new NotImplementedException();
        }

        public void LoadData(string path = "")
        {

        }
    }
}