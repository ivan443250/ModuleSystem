using UnityEngine;

namespace MVCSample.Infrastructure.DataHolding
{
    public interface IRepositoryDataReader<T> where T : class
    {
        void Read(T data);
    }
}