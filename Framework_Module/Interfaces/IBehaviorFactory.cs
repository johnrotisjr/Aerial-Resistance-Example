using Framework_Module.Definitions.Behaviors.Attack;

namespace Framework_Module.Interfaces
{
    public interface IBehaviorFactory<in T> where T : IWorldObject
    {
        public IAiBehavior<T> GetBehavior(AiBehaviorDefinition def);
    }
}