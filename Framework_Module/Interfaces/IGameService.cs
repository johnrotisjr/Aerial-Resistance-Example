
namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for services that require lifecycle management (initialization, shutdown).
    /// </summary>

    public interface IGameService
    {
        void Initialize();
        void Shutdown();
    }
}
