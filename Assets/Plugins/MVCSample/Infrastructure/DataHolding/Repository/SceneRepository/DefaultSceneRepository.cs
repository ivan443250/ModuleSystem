using MVCSample.Tools;
using System;
using System.Linq;

namespace MVCSample.Infrastructure.DataHolding
{
    public class DefaultSceneRepository : IRegistrator
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public DisposableObject Register(IModule objectToRegister)
        {
            if (TryGetGenericInterfaceType(objectToRegister, typeof(IRepositoryDataReader<>), out Type dataType) == false)
                return new(() => { });

            //throw new NotImplementedException();
            return new(() => { });
        }

        private bool TryGetGenericInterfaceType(IModule obj, Type genericInterface, out Type genericType)
        {
            genericType = null;

            var interfaceType = obj.GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface);

            if (interfaceType != null)
            {
                genericType = interfaceType.GetGenericArguments().FirstOrDefault();
                return genericType != null;
            }

            return false;
        }
    }
}