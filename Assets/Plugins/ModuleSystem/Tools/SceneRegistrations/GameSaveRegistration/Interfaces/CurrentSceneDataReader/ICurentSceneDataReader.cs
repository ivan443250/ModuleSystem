using System;

namespace MVCSample.Tools
{
    public class CurentSceneDataReaderTypes
    {
        public static Type[] All => new Type[]
        {
            typeof(ICurentSceneDataReader<>),
            typeof(ICurentSceneDataReader<,>),
            typeof(ICurentSceneDataReader<,,>),
        };
    }

    public interface ICurentSceneDataReader<T>
        where T : class, new()
    {
        void Read(T data);
    }

    public interface ICurentSceneDataReader<T1, T2>
        where T1 : class, new()
        where T2 : class, new()
    {
        void Read(T1 data1, T2 data2);
    }

    public interface ICurentSceneDataReader<T1, T2, T3>
        where T1 : class, new()
        where T2 : class, new()
        where T3 : class, new()
    {
        void Read(T1 data1, T2 data2, T3 data3);
    }
}
