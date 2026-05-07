using Framework_Module.Definitions;
using Framework_Module.Definitions.Behaviors;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Definitions.Behaviors.Movement;

namespace Framework_Module.Interfaces
{
    public interface IRebindableAiBehavior<in TDef> where TDef : AiBehaviorDefinition
    {
        public void Rebind(TDef aiBehaviorDefinition);
    }
}