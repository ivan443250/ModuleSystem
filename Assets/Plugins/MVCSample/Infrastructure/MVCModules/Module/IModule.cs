using MVCSample.Tools;

namespace MVCSample.Infrastructure
{
    public interface IModule : IDependencyCollectionElement
    {
        void Construct(Context context);
    }
}