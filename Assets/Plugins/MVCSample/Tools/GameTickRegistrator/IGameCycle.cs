namespace MVCSample.Tools
{
    public interface IGameCycle : ITickRegistrator
    {
        void Update(float tickDelta);
        void FixedUpdate(float fixedTickDelta);
    }
}
