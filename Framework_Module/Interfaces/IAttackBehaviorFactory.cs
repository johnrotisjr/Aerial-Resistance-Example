using Framework_Module.Configs.Ai;
using Framework_Module.Enums;

namespace Framework_Module.Interfaces
{
    public interface IAttackBehaviorFactory
    {
        public IAttackBehavior GetBehavior(AiAttackBehaviorConfig config);
    }
}