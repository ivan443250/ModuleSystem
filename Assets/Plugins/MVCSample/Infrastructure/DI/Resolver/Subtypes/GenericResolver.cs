using MVCSample.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MVCSample.Infrastructure.DI
{
    public class GenericResolver : IResolver, IResolvingChecker
    {
        public bool CheckResolving(Context context, Type type, out HashSet<Type> unresolvableTypes)
        {
            unresolvableTypes = new();

            IEnumerable<Type> allDependentTypes = GetInjectedMethods(type)
                .SelectMany(m => m.GetParameters())
                .Select(p => p.ParameterType);

            foreach (Type dependentType in allDependentTypes)
                if (context.HasBindingDeep(dependentType) == false)
                    unresolvableTypes.Add(dependentType);

            return unresolvableTypes.Count == 0;
        }

        public void Resolve(Context context, object resolvable)
        {
            foreach (MethodInfo method in GetInjectedMethods(resolvable.GetType()))
            {
                ParameterInfo[] parametersInfo = method.GetParameters();
                object[] parameters = new object[parametersInfo.Length];

                for (int i = 0; i < parametersInfo.Length; i++)
                    parameters[i] = GenericMethodInvoker
                        .Invoke(nameof(Context.ResolveDeep), context, parametersInfo[i].ParameterType);

                method.Invoke(resolvable, parameters);
            }
        }

        private IEnumerable<MethodInfo> GetInjectedMethods(Type type)
        {
            return type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<InjectFromContextAttribute>() != null);
        }
    }
}
