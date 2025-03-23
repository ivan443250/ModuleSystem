using System;
using UnityEngine;

namespace MVCSample.Infrastructure.DataHolding
{
    public class DefaultDataConverter : IDataConverter
    {
        public string ConvertFromObject(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public object ConvertToObject(string objectSrting, Type objectType)
        {
            return JsonUtility.FromJson(objectSrting, objectType);
        }

        public T ConvertToObject<T>(string objectSrting) where T : class 
        {
            return ConvertToObject(objectSrting, typeof(T)) as T;
        }
    }
}
