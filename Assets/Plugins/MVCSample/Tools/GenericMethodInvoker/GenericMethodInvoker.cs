using System;
using System.Reflection;
using UnityEngine;

namespace MVCSample.Tools
{
    public struct GenericMethodInvoker
    {
        public static object Invoke(string methodName, object methodOwner, Type genericType, params object[] parameters)
        {
            MethodInfo methodInfo = methodOwner
                .GetType()
                .GetMethod(methodName)
                .MakeGenericMethod(genericType);

            return methodInfo.Invoke(methodOwner, parameters);
        }
    }
}