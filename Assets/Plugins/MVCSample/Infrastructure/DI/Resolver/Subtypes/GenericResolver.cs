using MVCSample.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MVCSample.Infrastructure.DI
{
    public class GenericResolver : IResolver
    {
        private readonly Context _currentContext;

        public GenericResolver(Context currentContext)
        {
            _currentContext = currentContext;
        }

        public void Resolve(object resolvable, Type type)
        {
            foreach (MethodInfo method in GetInjectedMethods(type))
            {
                ParameterInfo[] parametersInfo = method.GetParameters();
                object[] parameters = new object[parametersInfo.Length];

                for (int i = 0; i < parametersInfo.Length; i++)
                {
                    parameters[i] = GenericMethodInvoker
                        .Invoke(nameof(Context.ResolveDependencyDeep), _currentContext, parametersInfo[i].ParameterType);
                }

                method.Invoke(resolvable, parameters);
            }
        }

        private IEnumerable<MethodInfo> GetInjectedMethods(Type type)
        {
            return type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.Name == "GetDependency" && m.GetCustomAttribute<InjectFromContextAttribute>() != null);
        }
    }
}
