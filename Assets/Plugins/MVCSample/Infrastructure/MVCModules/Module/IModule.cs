using MVCSample.Tools;
using System.Collections.Generic;

namespace MVCSample.Infrastructure
{
    public interface IModule : IDependencyCollectionElement
    {
        void Construct(Context context);
    }
}