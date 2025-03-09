using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Scripting;

namespace MVCSample.Infrastructure.DI
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class InjectFromContextAttribute : Attribute { }
}
