using System;

namespace MVCSample.Tools
{
    public class DeviceDataReaderTypes
    {
        public static Type[] All => new Type[]
        {
            typeof(IDeviceDataReader<>),
            typeof(IDeviceDataReader<,>),
            typeof(IDeviceDataReader<,,>),
        };
    }

    public interface IDeviceDataReader<T> 
        where T : class, new()
    {
        void Read(T data);
    }

    public interface IDeviceDataReader<T1, T2> 
        where T1 : class, new() 
        where T2 : class, new()
    {
        void Read(T1 data1, T2 data2);
    }

    public interface IDeviceDataReader<T1, T2, T3>
        where T1 : class, new()
        where T2 : class, new()
        where T3 : class, new()
    {
        void Read(T1 data1, T2 data2, T3 data3);
    }
}
