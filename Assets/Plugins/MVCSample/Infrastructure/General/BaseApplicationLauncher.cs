namespace MVCSample.Infrastructure
{
    public abstract class BaseApplicationLauncher
    {
        public void Launch()
        {
            PreInitialize();
            SetDependences();
            Initialize();

            InstallServicesBinding("binder");
            InstallModulesBinding("binder");
        }

        private void SetDependences() { }

        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }

        protected virtual void InstallServicesBinding(object binder) { }
        protected virtual void InstallModulesBinding(object binder) { }
    }
}
