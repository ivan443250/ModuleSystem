using System;

namespace MVCSample.Infrastructure.DI
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class InjectFromContextAttribute : Attribute { }
}
