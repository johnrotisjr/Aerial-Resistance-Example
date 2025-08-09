using Framework_Module.Interfaces;

namespace Framework_Module.Service
{
    /// <summary>
    /// Facade combining ServiceLocator and ServiceLifecycleManager.
    /// Provides centralized access and control over all game services.
    /// </summary>

    public class Services
    {
        private readonly ServiceLifecycleManager serviceLifecycleManager;
        private readonly ServiceLocator locator;
        public static Services Instance { get; } = new();

        private Services()
        {
            serviceLifecycleManager = new();
            locator = new();
        }
        
        public void Register<T>(IGameService service) where T : class, IGameService
        {
            locator.Register<T>(service);
            serviceLifecycleManager.TrackService<T>(service);
        }
        
        public void Unregister<T>() where T : class, IGameService
        {
            locator.Unregister<T>();
            serviceLifecycleManager.UntrackService<T>();
        }
        
        public T Get<T>() where T : class, IGameService
        {
            return locator.Get<T>();
        }
        
        public void InitializeServices()
        {
            serviceLifecycleManager.InitializeServices();
        }

        public void Shutdown()
        {
            serviceLifecycleManager.Shutdown();
            locator.ClearServices();
        }
    }
}
