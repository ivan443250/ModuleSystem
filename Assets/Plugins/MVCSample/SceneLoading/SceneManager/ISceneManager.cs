namespace MVCSample.SceneManagement
{
    public interface ISceneManager
    {
        SceneData GetSceneData(string sceneName);
        void AddScene(SceneData data, bool throwEx = true);
    }
}