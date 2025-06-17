using System;
using UnityEngine;

namespace MVCSample.Infrastructure.DataHolding
{
    public interface IDataConverter
    {
        string ConvertFromObject(object obj);

        object ConvertToObject(string objectSrting, Type objectType);
        T ConvertToObject<T>(string objectSrting) where T : class;
    }
}
