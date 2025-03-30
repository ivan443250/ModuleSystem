using MVCSample.Infrastructure;
using MVCSample.Infrastructure.DataHolding;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MVCSample.Tools
{
    public class DataReaderRegistrator : IRegistrator
    {
        private IDataExplorer _dataExplorer;

        public DataReaderRegistrator(IDataExplorer dataExplorer)
        {
            _dataExplorer = dataExplorer;
        }

        public DisposableObject Register(IModule objectToRegister)
        {
            SetDataInReaderTypes(objectToRegister, t => _dataExplorer.GlobalDataSet.GetData(t), DeviceDataReaderTypes.All);
            SetDataInReaderTypes(objectToRegister, t => _dataExplorer.SaveCellDataSet.GetData(t), GameSaveDataReaderTypes.All);
            SetDataInReaderTypes(objectToRegister, t => _dataExplorer.SceneDataSet.GetData(t), CurentSceneDataReaderTypes.All);

            return new();
        }

        private void SetDataInReaderTypes(IModule module, Func<Type, object> getDataCallback, params Type[] readerTypes)
        {
            foreach (Type type in readerTypes)
                SetDataInReader(module, type, getDataCallback);
        }

        private void SetDataInReader(IModule module, Type readerType, Func<Type, object> getDataCallback)
        {
            if (TryGetGenericInterfaceTypes(module, readerType, out Type[] genericTypes) == false)
                return;

            object[] dataSets = new object[genericTypes.Length];

            for (int i = 0; i < dataSets.Length; i++)
            {
                object dataSet = getDataCallback.Invoke(genericTypes[i]);

                if (dataSet == null)
                    dataSet = Activator.CreateInstance(genericTypes[i]);

                dataSets[i] = dataSet;
            }

            MethodInfo[] allMethods = readerType.MakeGenericType(genericTypes).GetMethods();

            if (allMethods.Length != 1)
                throw new Exception("1");

            //Debug.Log(string.Join<MethodInfo>(", ", allMethods));
            //MethodInfo genericMethodInfo = allMethods.First().MakeGenericMethod(genericTypes);

            allMethods.First().Invoke(module, dataSets);
        }

        private bool TryGetGenericInterfaceTypes(IModule obj, Type genericInterface, out Type[] genericTypes)
        {
            genericTypes = null;

            var interfaceType = obj.GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface);

            if (interfaceType != null)
            {
                genericTypes = interfaceType.GetGenericArguments();
                return true;
            }

            return false;
        }

        public async ValueTask DisposeAsync()
        {
            await _dataExplorer.SaveAll();
        }
    }
}