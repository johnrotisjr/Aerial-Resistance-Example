using Framework_Module.Configs.Ai;
using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IMovementBehaviorFactory
    {
        public IMovementBehavior GetBehavior(AiMovementBehaviorConfig config);
    }
}