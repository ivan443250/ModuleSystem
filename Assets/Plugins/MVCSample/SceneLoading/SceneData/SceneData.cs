using MVCSample.Infrastructure;

namespace MVCSample.SceneManagement
{
    public abstract class SceneData
    {
        public string Name { get; private set; }    

        public Context Context { get; private set; }

        public SceneData(string name, Context context)
        {
            Name = name;
            Context = context;
        }
    }
}
